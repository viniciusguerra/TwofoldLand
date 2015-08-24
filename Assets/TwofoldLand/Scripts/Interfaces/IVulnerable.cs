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
    [CodexDescription("Total magnitude of an Actor's resistance", 0)]
    float MaxHealth { get; }
    [CodexDescription("Current magnitude of an Actor's resistance. When it reaches 0, the actor is eliminated", 0)]
    float CurrentHealth { get; }

    [CodexDescription("Behaviour of the Actor when struck by an attack", 0)]
    AttackHandler AttackHandler
    {
        get;
    }
}