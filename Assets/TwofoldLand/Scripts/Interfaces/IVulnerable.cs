using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class AttackArgs : EventArgs
{
    public float rawDamage;
}

public delegate void AttackHandler(object sender, AttackArgs attackArgs);

public interface IVulnerable : IBase
{
    [CodexProperty(true, true)]
    float CurrentHealth { get; }
    [CodexProperty(true, true)]
    float MaxHealth { get; }

    AttackHandler AttackHandler
    {
        get;
    }
}