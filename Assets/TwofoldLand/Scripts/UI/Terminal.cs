using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System;
using System.Reflection;

public enum TerminalMessageMode
{
    Error,
    Success
}

public class Terminal : MonoBehaviour, ISubmitHandler, ISelectHandler
{
    [Header("Actor Selection")]
    public Actor selectedActor;
    public float actorSelectionRange = 4;

    [Header("Error Message Configuration")]
    public Color normalTextColor;
    public Color errorTextColor;
    public Color successTextColor;
    public float terminalMessageTime;

    private Stack<string> commandHistory;

    private InputField inputField;
    private Text inputFieldPlaceholder;
    private string originalPlaceholderText;
    private string completedCode;

    public delegate void ActorSelectionDelegate();
    public event ActorSelectionDelegate OnActorDeselection;

    public void SelectActor(Actor actor)
    {
        ClearActorSelection();

        selectedActor = actor;

        Ricci.Instance.LookAt(selectedActor.transform.position);
        Ricci.Instance.StopMoving();
        TargetMarker.Instance.HideInstantly();

        inputField.Select();
    }

    public void SetSelectedActor(Actor actor)
    {
        if (RicciIsInSelectionRange(actor.transform.position) && actor != null)
        {
            SelectActor(actor);
        }
    }

    public bool RicciIsInSelectionRange(Vector3 target)
    {
        return Vector3.Distance(Ricci.Instance.gameObject.transform.position, target) < actorSelectionRange;
    }

    public bool HasSelectedActor()
    {
        return selectedActor != null;
    }

    public void ClearActorSelection()
    {
        selectedActor = null;
        OnActorDeselection();
    }

    public void SubmitCommandToSelectedActor()
    {
        try
        {
            Command command = Command.BuildCommand(inputField.text);

            try
            {
                selectedActor.SubmitCommand(command);                
            }
#pragma warning disable 0168
            catch (MethodAccessException mae)
            {
                ShowMessage(GlobalDefinitions.UnnaccessibleMethodErrorMessage, TerminalMessageMode.Error);
            }
            catch (NotImplementedException nie)
            {
                ShowMessage(GlobalDefinitions.InvalidMethodErrorMessage, TerminalMessageMode.Error);
            }
            catch (MissingMethodException mme)
            {
                ShowMessage(GlobalDefinitions.InvalidMethodErrorMessage, TerminalMessageMode.Error);
            }
            catch (TargetParameterCountException tpce)
#pragma warning restore 0168
            {
                ShowMessage(GlobalDefinitions.InvalidParametersErrorMessage, TerminalMessageMode.Error);
            }

            SetInputFieldText(string.Empty);
        }
#pragma warning disable 0168
        catch (WrongCommandSyntaxException wcs)
        {
            ShowMessage(GlobalDefinitions.WrongSyntaxErrorMessage, TerminalMessageMode.Error);
        }        
        catch (MissingMemberException mme)
        {
            ShowMessage(GlobalDefinitions.InvalidMethodErrorMessage, TerminalMessageMode.Error);
        }
#pragma warning restore 0168
    }

    private void HandleInputs()
    {
        if (HasSelectedActor())
        {
            if (!RicciIsInSelectionRange(selectedActor.transform.position) || Input.GetMouseButtonUp(1))
                ClearActorSelection();
        }

        if(inputField.isActiveAndEnabled)
        {
            if (Input.GetKeyUp(KeyCode.Return))
            {
                ExecuteEvents.Execute<ISubmitHandler>(gameObject, null, (x, y) => x.OnSubmit(null));
            }

            if (inputField.text.Equals(string.Empty) && Input.GetKeyUp(KeyCode.UpArrow))
            {
                SetInputFieldText(commandHistory.Peek());
                inputField.MoveTextEnd(false);

                SetPlaceholderText(inputField.text);
            }

            if (Input.GetKeyUp(KeyCode.Tab) || Input.GetKeyUp(KeyCode.RightArrow) && !Regex.IsMatch(inputField.text, GlobalDefinitions.TerminalCommandRegexPattern))
            {
                SetInputFieldText(completedCode);
                inputField.MoveTextEnd(false);
            }
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.Return))
                inputField.Select();
        }
    }

    public void ResetPlaceholderIfEmpty()
    {
        if (inputField.text.Equals(string.Empty))
            SetPlaceholderText(originalPlaceholderText);
    }

    private void SetPlaceholderText(string text)
    {
        inputFieldPlaceholder.text = text;
    }

    private void SetInputFieldText(string text)
    {
        inputField.text = text;
        inputField.MoveTextEnd(false);
    }

    public void CodeCompletion()
    {
        inputField.placeholder.enabled = true;

        if (inputField.text.Equals(string.Empty))
        {
            SetPlaceholderText(originalPlaceholderText);
            return;
        }

        if (Regex.IsMatch(inputField.text, GlobalDefinitions.TerminalInterfaceRegexPattern))
        {
            try
            {
                completedCode = Ricci.Instance.FindInterfaceStartingWith(inputField.text).Name;
            }
#pragma warning disable 0168
            catch (MissingMemberException mme)
#pragma warning restore 0168
            {
                completedCode = inputField.text;
            }
            finally
            {
                SetPlaceholderText(completedCode);
            }
        }

        if (Regex.IsMatch(inputField.text, GlobalDefinitions.TerminalInterfaceAndMethodRegexPattern))
        {
            string[] commandSplit = inputField.text.Split('.');

            completedCode = inputField.text;

            try
            {
                string remainder = Ricci.Instance.FindRemainderOfMethodStartingWith(Ricci.Instance.FindInterface(commandSplit[0]), commandSplit[1]);

                if(!string.IsNullOrEmpty(remainder))
                    remainder += "()";

                completedCode += remainder;
            }
#pragma warning disable 0168
            catch (MissingMemberException mme)
#pragma warning restore 0168
            {

            }

            SetPlaceholderText(completedCode);
        }
    }

    public void ShowMessage(string message, TerminalMessageMode messageMode)
    {
        StartCoroutine(ShowMessageCoroutine(message, messageMode));
    }

    public IEnumerator ShowMessageCoroutine(string message, TerminalMessageMode messageMode)
    {
        switch (messageMode)
        {
            case TerminalMessageMode.Error:
                {
                    inputField.textComponent.color = errorTextColor;
                    break;
                }
            case TerminalMessageMode.Success:
                {
                    inputField.textComponent.color = successTextColor;
                    break;
                }
            default:
                break;
        }

        SetPlaceholderText(string.Empty);
        SetInputFieldText(message);

        float counter = 0;

        do
        {
            counter += Time.deltaTime;

            if (inputField.text != message)
            {
                break;
            }

            yield return null;
        } while (counter < terminalMessageTime);

        if (inputField.text == message)
            SetInputFieldText(string.Empty);

        inputField.textComponent.color = normalTextColor;
    }

    public void OnSelect(BaseEventData eventData)
    {
        SetPlaceholderText(string.Empty);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (inputField.text != string.Empty)
        {
            commandHistory.Push(inputField.text);

            if (selectedActor != null)
                SubmitCommandToSelectedActor();
            else
                ShowMessage(GlobalDefinitions.NoTargetErrorMessage, TerminalMessageMode.Error);
        }
    }

    void Start()
    {
        inputField = GetComponent<InputField>();
        inputFieldPlaceholder = (Text)inputField.placeholder;

        OnActorDeselection = delegate { };

        commandHistory = new Stack<string>();
        commandHistory.Push(string.Empty);

        originalPlaceholderText = ((Text)inputField.placeholder).text;
    }

    void Update()
    {
        HandleInputs();
    }
}
