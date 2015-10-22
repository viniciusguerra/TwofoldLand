using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

public class Sign : Entity, IVerbal
{    
    public string[] messages;
    public UnityEvent onMessageEnd;

    public string[] Messages { get { return messages; } }

    public void OnMessageEnd()
    {
        onMessageEnd.Invoke();
    }
}
