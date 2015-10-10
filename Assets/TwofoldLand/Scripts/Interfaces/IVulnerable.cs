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
    [CodexProperty("Total magnitude of an Actor's resistance", true)]
    float CurrentHealth { get; }
    [CodexProperty("Current magnitude of an Actor's resistance. When it reaches 0, the actor is eliminated", true)]
    float MaxHealth { get; }

    AttackHandler AttackHandler
    {
        get;
    }
}