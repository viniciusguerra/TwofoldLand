using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;

public class Ricci : Singleton<Ricci>
{
    #region Properties
    public float actorSelectionRange = 4;

    [Header("Skills")]
    public List<Skill> skillList;

    [Header("Aurum")]
    public int availableAura;

    //[Header("Spells")]
    //public List<Spell> spellList;

    private NavMeshAgent agent;
    #endregion

    #region Methods
    public void AddInterface(Skill interfaceContainer)
    {
        skillList.Add(interfaceContainer);
    }

    public bool KnowsInterface(Type interfaceType)
    {
        foreach(Skill s in skillList)
        {
            if (s.interfaceContainer.InterfaceType == interfaceType)
                return true;
        }

        return false;
    }

    ///<exception cref="MissingMemberException">Thrown when there is no existent interface with the given name</exception>
    public Type FindInterface(string interfaceName)
    {
        foreach (Skill s in skillList)
        {
            if (s.interfaceContainer.InterfaceType.Name.Equals(interfaceName))
                return s.interfaceContainer.InterfaceType;
        }

        throw new MissingMemberException();
    }

    ///<exception cref="MissingMemberException">Thrown when there is no existent interface with the given name</exception>
    public Type FindInterfaceStartingWith(string text)
    {
        foreach(Skill s in skillList)
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

            foreach(Skill s in skillList)
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

    public bool IsInSelectionRange(Vector3 target)
    {
        return Vector3.Distance(Ricci.Instance.gameObject.transform.position, target) < actorSelectionRange;
    }

    private void CollectAura(int amount)
    {
        availableAura += amount;

        HUD.Instance.UpdateAuraUI(amount);
        HUD.Instance.log.Push("Acquired " + amount + " aura");
    }
    #endregion

    #region MonoBehaviour
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == GlobalDefinitions.AuraTag)
        {
            AuraContainer auraContainer = collision.gameObject.GetComponent<AuraContainer>();

            CollectAura(auraContainer.Amount);

            auraContainer.Destroy();
        }
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        HUD.Instance.UpdateAuraUI(availableAura);
    }
    #endregion
}