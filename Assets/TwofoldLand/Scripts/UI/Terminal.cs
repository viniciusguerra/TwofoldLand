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

public class Terminal : UIWindow, ISubmitHandler, ISelectHandler
{
    #region Properties
    [Header("Actor Selection")]
    public Actor selectedActor;

    [Header("Error Message Configuration")]
    public Color normalTextColor;
    public Color errorTextColor;
    public Color successTextColor;
    public float terminalMessageTime;

    [HideInInspector]
    public Stack<string> commandHistory;

    [HideInInspector]
    public InputField inputField;
    [HideInInspector]
    public Text inputFieldPlaceholder;
    [HideInInspector]
    public string originalPlaceholderText;
    [HideInInspector]
    public string completedCode;

    private bool codeCompleteIsAddressContent = false;
    #endregion

    #region Methods
    public void SetSelectedActor(Actor actor)
    {
        if (Player.Instance.CanReach(actor.gameObject) && actor != null)
        {
            if (actor != selectedActor)
            {
                ClearActorSelection();

                selectedActor = actor;

                MainCamera.Instance.EnableOutline();
            }

            Player.Instance.MovementController.LookAt(selectedActor.transform.position);
            Player.Instance.MovementController.StopMoving();
            TargetMarker.Instance.HideInstantly();

            inputField.Select();
        }
    }

    public void ClearActorSelection()
    {
        if (HasSelectedActor())
        {
            selectedActor.Deselect();
        }

        MainCamera.Instance.DisableOutline();

        selectedActor = null;  
    }

    public bool HasSelectedActor()
    {
        return selectedActor != null;
    }

    public void SubmitCommandToSelectedActor()
    {
        if (Regex.IsMatch(inputField.text, GlobalDefinitions.StorageAddressPattern))
        {
            Spell spellAtAddress = HUD.Instance.storage.GetSpellFromAddress(inputField.text);

            if (spellAtAddress != null)
            {
                if (Player.Instance.Entity.CurrentStamina >= spellAtAddress.StaminaCost)
                {
                    selectedActor.Entity.ExecuteSpell(spellAtAddress);
                }
                else
                {
                    HUD.Instance.log.ShowMessage(GlobalDefinitions.NotEnoughStaminaErrorMessage);
                }
            }
            else
            {
                ShowMessage(GlobalDefinitions.InvalidAddressErrorMessage, TerminalMessageMode.Error);
            }

            SetInputFieldText(string.Empty);

            return;
        }

        try
        {
            Command command = Command.BuildCommand(Player.Instance.Entity, inputField.text);

            try
            {
                if (Player.Instance.Entity.CurrentStamina >= command.staminaCost)
                {
                    selectedActor.Entity.ExecuteCommand(command);
                }
                else
                {
                    HUD.Instance.log.ShowMessage(GlobalDefinitions.NotEnoughStaminaErrorMessage);
                }
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

    //Input Field
    public void ResetPlaceholderIfEmpty()
    {
        if (inputField.text.Equals(string.Empty))
            SetPlaceholderText(originalPlaceholderText);
    }

    public void SetPlaceholderText(string text)
    {
        inputFieldPlaceholder.text = text;
    }

    public void SetInputFieldText(string text)
    {
        inputField.text = text;
        inputField.MoveTextEnd(false);
    }

    public void CompleteInputField()
    {
        if (!codeCompleteIsAddressContent)
        {
            HUD.Instance.terminal.SetInputFieldText(HUD.Instance.terminal.completedCode);
            HUD.Instance.terminal.inputField.MoveTextEnd(false);
        }
    }

    public void CodeCompletion()
    {
        inputField.placeholder.enabled = true;

        if (inputField.text.Equals(string.Empty))
        {
            SetPlaceholderText(originalPlaceholderText);
            return;
        }

        if (Regex.IsMatch(inputField.text, GlobalDefinitions.StorageAddressPattern))
        {
            codeCompleteIsAddressContent = false;

            Spell spellAtAddress = HUD.Instance.storage.GetSpellFromAddress(inputField.text);
            completedCode = inputField.text;

            if (spellAtAddress != null)
            {
                completedCode += " " + spellAtAddress.SpellTitle;
                codeCompleteIsAddressContent = true;
            }

            SetPlaceholderText(completedCode);

            return;
        }

        if (Regex.IsMatch(inputField.text, GlobalDefinitions.TerminalInterfaceRegexPattern))
        {
            codeCompleteIsAddressContent = false;

            try
            {
                completedCode = Player.Instance.FindInterfaceStartingWith(inputField.text).Name;
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

            return;
        }

        if (Regex.IsMatch(inputField.text, GlobalDefinitions.TerminalInterfaceAndMethodRegexPattern))
        {
            codeCompleteIsAddressContent = false;

            string[] commandSplit = inputField.text.Split('.');

            completedCode = inputField.text;

            try
            {
                string methodName = Player.Instance.GetMethodNameStartingWith(Player.Instance.FindInterface(commandSplit[0]), commandSplit[1]);

                if (!string.IsNullOrEmpty(commandSplit[1]))
                {
                    methodName = methodName.Replace(commandSplit[1], String.Empty);
                    methodName += "()";
                }

                completedCode += methodName;
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

            return;
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
    #endregion

    #region MonoBehaviour
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

    public override void Start()
    {
        base.Start();

        inputField = GetComponent<InputField>();
        inputFieldPlaceholder = (Text)inputField.placeholder;

        commandHistory = new Stack<string>();
        commandHistory.Push(string.Empty);

        originalPlaceholderText = ((Text)inputField.placeholder).text;
    }
    #endregion
}
