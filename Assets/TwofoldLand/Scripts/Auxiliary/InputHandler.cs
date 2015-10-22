﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
	#region Methods
    private void HandleInputs()
    {
        //Terminal
        if (HUD.Instance.terminal.HasSelectedActor())
        {
            if (Input.GetMouseButtonUp(1))
            {
                HUD.Instance.terminal.ClearActorSelection();
                HUD.Instance.binaryConversion.Hide();
                HUD.Instance.messageBox.Hide();
            }
        }
        else
        {
            //Ricci
            if (Input.GetMouseButtonUp(1))
            {
                Player.Instance.MovingEntity.StopMoving();
                TargetMarker.Instance.HideInstantly();                
            }
        }

        if (HUD.Instance.terminal.inputField.isActiveAndEnabled)
        {
            if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter))
            {
                ExecuteEvents.Execute<ISubmitHandler>(HUD.Instance.terminal.gameObject, null, (x, y) => x.OnSubmit(null));
            }

            if (HUD.Instance.terminal.inputField.text.Equals(string.Empty) && Input.GetKeyUp(KeyCode.UpArrow))
            {
                HUD.Instance.terminal.SetInputFieldText(HUD.Instance.terminal.commandHistory.Peek());
                HUD.Instance.terminal.inputField.MoveTextEnd(false);

                HUD.Instance.terminal.SetPlaceholderText(HUD.Instance.terminal.inputField.text);
            }

            if (Input.GetKeyUp(KeyCode.Tab) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                HUD.Instance.terminal.CompleteInputField();                
            }
        }
        else
        {
            //if (Input.GetButton(GlobalDefinitions.CodexButton))
            //    HUD.Instance.codex.Toggle();            

            //if (Input.GetButton(GlobalDefinitions.StorageButton))
            //    HUD.Storage.Toggle();            

            //if (Input.GetButton(GlobalDefinitions.NotesButton))
            //    HUD.Notes.Toggle();            

            if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter))
                HUD.Instance.terminal.inputField.Select();
        }

        if(TutorialPanelManager.Instance.IsShowingTutorial)
        {
            if (Input.GetKey(KeyCode.Escape))
                TutorialPanelManager.Instance.Hide();
        }
    }
	#endregion

	#region MonoBehaviour
    void Update()
    {
        HandleInputs();
    }
	#endregion
}
