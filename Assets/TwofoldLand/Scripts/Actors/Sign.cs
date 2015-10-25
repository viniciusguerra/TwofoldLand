using UnityEngine;
using UnityEngine.Events;
using SmartLocalization;
using System.Collections;
using System;

public class Sign : Entity, IVerbal
{
    public string[] messageKeys;
    public UnityEvent onMessageEnd;    

    public string[] Messages
    {
        get
        {
            return LanguageManager.Instance.GetTextValuesForKeys(messageKeys);
        }
    }

    public void OnMessageEnd()
    {
        onMessageEnd.Invoke();
    }
}
