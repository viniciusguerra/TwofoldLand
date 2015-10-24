using UnityEngine;
using System.Collections;
using System.Reflection;
using System;
using System.Collections.Generic;

public class Entity : MonoBehaviour
{
    protected Command currentCommand;
    protected Spell currentSpell;

    private Type[] implementedInterfaces;
    private List<Type> knownInterfaceList;

    public List<Type> KnownInterfaceList
    {
        get
        {
            UpdateKnownInterfaceList();

            return knownInterfaceList;
        }
    }

    private void UpdateKnownInterfaceList()
    {
        if (knownInterfaceList == null)
            knownInterfaceList = new List<Type>();

        if (implementedInterfaces == null)
            implementedInterfaces = GetType().GetInterfaces();

        knownInterfaceList.Clear();

        for (int i = 0; i < implementedInterfaces.Length; i++)
        {
            if (Player.Instance.KnowsInterface(implementedInterfaces[i]))
            {
                knownInterfaceList.Add(implementedInterfaces[i]);
            }
        }
    }

    public bool ExecuteCommand(Command command)
    {
        if (currentCommand == null)
        {
            currentCommand = command;
            currentCommand.Execute(this);

            return true;
        }
        else
        {
            Debug.Log("Entity: Another Command already being executed (sender: " + currentCommand.Sender.name + ", method name: " + currentCommand.methodInfo.Name + ")");

            return false;
        }
    }

    public virtual void OnCommandFailure(string message)
    {
        HUD.Instance.log.ShowMessage(message);

        if (currentSpell != null)
            OnSpellEnd();

        OnCommandEnd();
    }

    public virtual void OnCommandSuccess()
    {
        currentCommand.Sender.CurrentStamina -= currentCommand.staminaCost;

        OnCommandEnd();
    }

    public virtual void OnCommandEnd()
    {
        currentCommand = null;
    }

    public virtual void ExecuteSpell(Spell spell)
    {
        currentSpell = spell;

        StartCoroutine(SpellExecutionCoroutine(spell));
    }

    public virtual void OnSpellEnd()
    {
        currentSpell.sender.CurrentStamina -= currentSpell.StaminaCost;

        currentSpell = null;
    }

    private IEnumerator SpellExecutionCoroutine(Spell spell)
    {
        foreach (Command c in spell.commands)
        {
            while (currentCommand != null)
                yield return null;

            ExecuteCommand(c);
        }

        OnSpellEnd();
    }
}
