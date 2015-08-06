using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using System.Linq;

[System.Serializable]
public class Spell
{
    [SerializeField]
    private string spellTitle;

    public string SpellTitle
    {
        get
        {
            return spellTitle;
        }
        set
        {
            spellTitle = value + (!value.EndsWith(GlobalDefinitions.SpellSuffix) ? GlobalDefinitions.SpellSuffix : string.Empty);
        }
    }

    [SerializeField]
    private string spellWords;

    public string SpellWords
    {
        get
        {
            return spellWords;
        }
        set
        {
            spellWords = value;

            UpdateCommands();
        }
    }

    [SerializeField]
    private int auraCost;

    public int AuraCost
    {
        get
        {
            auraCost = CalculateAuraCost();

            return auraCost;
        }
        private set
        {
            auraCost = value;
        }
    }

    [SerializeField]
    private int staminaCost;

    public int StaminaCost
    {
        get
        {
            staminaCost = CalculateStaminaCost();

            return staminaCost;
        }
        private set
        {
            staminaCost = value;
        }
    }

    private int CalculateAuraCost()
    {
        if (commands == null)
            return 0;

        int auraCost = 0;

        foreach(Command c in commands)
        {
            auraCost += c.auraCostToCompile;
        }

        return auraCost;
    }

    private int CalculateStaminaCost()
    {
        if (commands == null)
            return 0;

        int staminaCost = 0;

        foreach (Command c in commands)
        {
            staminaCost += c.staminaCost;
        }

        return staminaCost;
    }

    public Command[] commands;

    public void UpdateCommands()
    {
        List<Command> commandList = new List<Command>();

        string[] spellLineSplit = Regex.Split(spellWords, @"\s+").Where(s => s != string.Empty).ToArray<string>();

        foreach (string line in spellLineSplit)
        {
            try
            {
                Command c = Command.BuildCommand(line);
                commandList.Add(c);
            }
            catch(WrongCommandSyntaxException ex0)
            {
                //Debug.Log(GlobalDefinitions.WrongSyntaxErrorMessage);
            }            
            catch(MissingMethodException ex2)
            {
                //Debug.Log(GlobalDefinitions.InvalidMethodErrorMessage);
            }
            catch (MissingMemberException ex1)
            {
                //Debug.Log(GlobalDefinitions.InvalidInterfaceErrorMessage);
            }
        }

        commands = commandList.ToArray();
    }

    public Spell(Command[] commands)
    {
        this.commands = commands;
    }

    public Spell()
    {

    }
}
