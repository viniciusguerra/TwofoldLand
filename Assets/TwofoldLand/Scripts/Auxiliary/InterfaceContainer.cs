using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class InterfaceContainer : ScriptableObject
{
    public virtual Type InterfaceType { get { return null; } }
}
