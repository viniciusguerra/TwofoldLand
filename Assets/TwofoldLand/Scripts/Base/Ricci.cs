using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;

public class Ricci : SingletonMonoBehaviour<Ricci>
{
    public List<Skill> skills;

    //[Header("Spells")]
    //public List<Spell> spellList;

    private NavMeshAgent agent;

    public static List<Skill> Skills
    {
        get
        {
            return Instance.skills;
        }
    }

    public void AddInterface(Skill interfaceContainer)
    {
        skills.Add(interfaceContainer);
    }

    public bool KnowsInterface(Type interfaceType)
    {
        foreach(Skill s in skills)
        {
            if (s.interfaceContainer.InterfaceType == interfaceType)
                return true;
        }

        return false;
    }

    ///<exception cref="MissingMemberException">Thrown when there is no existent interface with the given name</exception>
    public Type FindInterface(string interfaceName)
    {
        foreach (Skill s in skills)
        {
            if (s.interfaceContainer.InterfaceType.Name.Equals(interfaceName))
                return s.interfaceContainer.InterfaceType;
        }

        throw new MissingMemberException();
    }

    ///<exception cref="MissingMemberException">Thrown when there is no existent interface with the given name</exception>
    public Type FindInterfaceStartingWith(string text)
    {
        foreach(Skill s in skills)
        {
            if (s.interfaceContainer.InterfaceType.Name.StartsWith(text))
                return s.interfaceContainer.InterfaceType;
        }

        throw new MissingMemberException();
    }

    public string FindRemainderOfMethodStartingWith(Type interfaceType, string text)
    {
        if (!string.IsNullOrEmpty(text))
        {            
            string remainder = string.Empty;

            foreach(Skill s in skills)
            {
                if (s.interfaceContainer.InterfaceType == interfaceType)
                {
                    foreach (MethodInfo m in s.interfaceContainer.InterfaceType.GetMethods())
                    {
                        if (m.Name.StartsWith(text))
                            remainder = m.Name.Replace(text, String.Empty);
                    }
                }
            }            

            return remainder;
        }
        else
            return string.Empty;
    }

    public void LookAt(Vector3 target)
    {
        transform.LookAt(target);    
    }

    public void MoveToPosition(Vector3 targetPosition)
    {        
        agent.SetDestination(targetPosition);
        agent.Resume();
    }

    public void StopMoving()
    {
        agent.Stop();
    }

    private void HandleInputs()
    {        
        if (!HUD.Terminal.HasSelectedActor() && Input.GetMouseButtonUp(1))
        {
            Ricci.Instance.StopMoving();
            TargetMarker.Instance.Hide();
        }        
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        HandleInputs();
    }
}