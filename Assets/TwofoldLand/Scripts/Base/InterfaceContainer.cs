using UnityEngine;
using System.Collections;
using System;

public class InterfaceContainer : ScriptableObject
{
    public virtual Type InterfaceType { get { return null; } }
}
