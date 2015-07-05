using UnityEngine;
using System.Collections;
using System;

public class Skill : ScriptableObject
{
    public int level;
    public virtual Type InterfaceType { get { return null; } }
}
