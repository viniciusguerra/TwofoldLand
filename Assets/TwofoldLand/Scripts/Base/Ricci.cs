using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;

public class Ricci : Singleton<Ricci>, IDamageable
{
    #region Properties
    public float actorSelectionRange = 4;

    [Header("Skills")]
    public List<Skill> skillList;

    [SerializeField]
    private bool compilerAvailable = false;
    public bool IsCompilerAvailable
    {
        get
        {
            return compilerAvailable;
        }
        private set
        {
            compilerAvailable = value;

            if (compilerAvailable)
                Rest();
        }
    }

    [Header("Aurum")]
    [SerializeField]
    private int availableAura;

    public int Aura
    {
        get { return availableAura; }
    }

    [Header("IDamageable")]
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float currentHealth;

    public float Health
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = Mathf.Max(0, value);

            HUD.Instance.SetHealthBarValue(currentHealth);
        }
    }

    public float MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            maxHealth = Mathf.Max(0, value);

            HUD.Instance.SetMaxHealth(maxHealth);
        }
    }

    [Header("Stamina")]
    [SerializeField]
    private float maxStamina;
    [SerializeField]
    private float currentStamina;

    public float staminaRegenRate = 0.2f;
    public float staminaRegenWait = 1;
    private float currentStaminaRegenWait;
    private bool regeneratingStamina = false;

    public float Stamina
    {
        get
        {
            return currentStamina;
        }
        set
        {
            currentStamina = Mathf.Max(0, value);           

            HUD.Instance.SetStaminaBarValue(currentStamina);

            //if (currentStamina < MaxStamina && !regeneratingStamina)
            //{
            //    currentStaminaRegenWait = 0;
            //    StartCoroutine(StaminaRegen());
            //}
        }
    }

    public float MaxStamina
    {
        get
        {
            return maxStamina;
        }
        set
        {
            maxStamina = Mathf.Max(0, value);

            HUD.Instance.SetMaxStamina(maxStamina);
        }
    }

    private NavMeshAgent agent;
    #endregion

    #region Methods
    public void AddInterface(Skill interfaceContainer)
    {
        skillList.Add(interfaceContainer);
    }

    public bool KnowsInterface(Type interfaceType)
    {
        foreach (Skill s in skillList)
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
        foreach (Skill s in skillList)
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

            foreach (Skill s in skillList)
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

    public void CollectAura(int amount)
    {
        availableAura += Mathf.Abs(amount);

        HUD.Instance.UpdateAuraUI(availableAura);
        HUD.Instance.log.Push("Acquired " + Mathf.Abs(amount) + " aura");
    }

    public void SpendAura(int amount)
    {
        availableAura -= Mathf.Abs(amount);

        HUD.Instance.UpdateAuraUI(availableAura);
        HUD.Instance.log.Push("Spent " + Mathf.Abs(amount) + " aura");
    }

    //TODO Stamina Regen
    //private IEnumerator StaminaRegen()
    //{
    //    regeneratingStamina = true;

    //    while (currentStaminaRegenWait < staminaRegenWait)
    //    {
    //        currentStaminaRegenWait += Time.deltaTime;
    //        yield return null;
    //    }

    //    while (Stamina <= MaxStamina)
    //    {
    //        Stamina += MaxStamina * staminaRegenRate * Time.deltaTime;
    //        yield return null;
    //    }

    //    regeneratingStamina = false;
    //}

    public void Rest()
    {
        Health = MaxHealth;
        Stamina = MaxStamina;
    }
    #endregion

    #region MonoBehaviour
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == GlobalDefinitions.AuraTag)
        {
            AuraContainer auraContainer = collision.gameObject.GetComponent<AuraContainer>();

            CollectAura(auraContainer.Amount);

            auraContainer.Destroy();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == GlobalDefinitions.CompilerTag)
        {
            IsCompilerAvailable = true;
            HUD.Instance.ide.SetCompileButton();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == GlobalDefinitions.CompilerTag)
        {
            IsCompilerAvailable = false;
            HUD.Instance.ide.SetCompileButton();
        }
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        HUD.Instance.UpdateAuraUI(availableAura);

        HUD.Instance.SetMaxHealth(maxHealth);
        HUD.Instance.SetMaxStamina(maxStamina);        

        HUD.Instance.UpdateHealthBarValue(Health);
        HUD.Instance.UpdateStaminaBarValue(Stamina);

        compilerAvailable = false;        
    }    
    #endregion
}