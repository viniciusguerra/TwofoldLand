using UnityEngine;
using System.Collections;

[System.Serializable]
public class Spell
{
    public string SpellName
    {
        get
        {
            return SpellName;
        }
        set
        {
            SpellName = value + GlobalDefinitions.SpellSuffix;
        }
    }

    public Command[] commands;

    public Spell(Command[] commands)
    {
        this.commands = commands;
    }
}
