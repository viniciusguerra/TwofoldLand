using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;

[RequireComponent(typeof(PlayerEntity), typeof(MovementController))]
public class Player : Singleton<Player>, IVulnerable
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

    public float CurrentHealth
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

    private AttackHandler attackHandler;

    public AttackHandler AttackHandler
    {
        private set
        {
            attackHandler = value;
        }
        get
        {
            return attackHandler;
        }
    }

    private PlayerEntity entity;

    public PlayerEntity Entity { get { return entity; } }

    private MovementController movementController;

    public MovementController MovementController { get { return movementController; } }
    #endregion

    #region Skill Methods
    public void AddSkill(SkillData skillData)
    {
        if (skillList.Exists(x => x.GetInterfaceType() == skillData.InterfaceType))
            return;

        Skill skill = new Skill(skillData);

        skillList.Add(skill);

        HUD.Instance.collectableAcquiredWindow.ShowSkillAcquired(skill);
    }

    public Skill GetSkill(Type interfaceType)
    {
        return skillList.Find(x => x.GetInterfaceType().Equals(interfaceType));
    }

    public Skill GetSkill(string interfaceName)
    {
        return skillList.Find(x => x.GetInterfaceType().Name.Equals(interfaceName));
    }

    public Type[] GetAllSkillInterfaces()
    {
        Type[] interfaces = new Type[skillList.Count];

        for (int i = 0; i < skillList.Count; i++)
        {
            interfaces[i] = skillList[i].GetInterfaceType();
        }

        return interfaces;
    }

    public void LevelSkillUp(Type interfaceType)
    {
        Skill skillToLevel = skillList.Find(x => x.GetInterfaceType().Equals(interfaceType));
        int costToLevelUp = skillToLevel.GetCostToLevelUp();

        if (Aura >= costToLevelUp)
        {
            SpendAura(costToLevelUp);
            skillToLevel.LevelUp();

            HUD.Instance.collectableAcquiredWindow.ShowSkillLeveledUp(skillToLevel);
        }
        else
            HUD.Instance.log.ShowMessage("Not enough Aura to level " + skillToLevel.GetInterfaceType().Name + " up");
    }

    public bool KnowsInterface(Type interfaceType)
    {
        foreach (Skill s in skillList)
        {
            if (s.GetInterfaceType() == interfaceType)
                return true;
        }

        return false;
    }

    ///<exception cref="MissingMemberException">Thrown when there is no existent interface with the given name</exception>
    public Type FindInterface(string interfaceName)
    {
        foreach (Skill s in skillList)
        {
            if (s.GetInterfaceType().Name.Equals(interfaceName))
                return s.GetInterfaceType();
        }

        throw new MissingMemberException();
    }

    ///<exception cref="MissingMemberException">Thrown when there is no existent interface with the given name</exception>
    public Type FindInterfaceStartingWith(string text)
    {
        foreach (Skill s in skillList)
        {
            if (s.GetInterfaceType().Name.StartsWith(text))
                return s.GetInterfaceType();
        }

        throw new MissingMemberException();
    }

    public string GetMethodNameStartingWith(Type interfaceType, string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            string methodName = string.Empty;

            foreach (Skill s in skillList)
            {
                if (s.GetInterfaceType() == interfaceType)
                {
                    foreach (MethodInfo m in s.GetInterfaceType().GetMethods())
                    {
                        if (m.Name.StartsWith(text))
                            methodName = m.Name;
                    }
                }
            }

            return methodName;
        }
        else
            return string.Empty;
    }
    #endregion

    #region Methods
    public bool CanReach(GameObject target)
    {
        RaycastHit hitInfo;
        Ray reachRay = new Ray(transform.position, target.transform.position - transform.position);

        Physics.Raycast(reachRay, out hitInfo, actorSelectionRange);

        if (hitInfo.collider != null && (hitInfo.collider.gameObject == target || hitInfo.transform.IsChildOf(target.transform)))
            return true;
        else
            return false;
    }

    public void CollectAura(int amount)
    {
        availableAura += Mathf.Abs(amount);

        HUD.Instance.UpdateAuraUI(availableAura);
        HUD.Instance.log.ShowMessage("Acquired " + Mathf.Abs(amount) + " aura");

        if (HUD.Instance.codex.IsVisible)
            HUD.Instance.codex.UpdateInterfaceLevelArea();
    }

    public void SpendAura(int amount)
    {
        availableAura -= Mathf.Abs(amount);

        HUD.Instance.UpdateAuraUI(availableAura);
        HUD.Instance.log.ShowMessage("Spent " + Mathf.Abs(amount) + " aura");

        if (HUD.Instance.codex.IsVisible)
            HUD.Instance.codex.UpdateInterfaceLevelArea();
    }

    public void Rest()
    {
        CurrentHealth = MaxHealth;
        Entity.CurrentStamina = Entity.MaxStamina;
    }
    #endregion

    #region MonoBehaviour
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == GlobalDefinitions.CollectableTag)
        {
            Collectable collectable = collision.gameObject.GetComponent<Collectable>();

            if (collectable.CollectOnTouch)
                collision.gameObject.GetComponent<Collectable>().Absorb();
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

    void Awake()
    {
        entity = GetComponent<PlayerEntity>();
        movementController = GetComponent<MovementController>();
    }

    void Start()
    {
        HUD.Instance.UpdateAuraUI(availableAura);

        HUD.Instance.SetMaxHealth(maxHealth);
        HUD.Instance.SetMaxStamina(Entity.MaxStamina);

        HUD.Instance.UpdateHealthBarValue(CurrentHealth);
        HUD.Instance.UpdateStaminaBarValue(Entity.CurrentStamina);

        compilerAvailable = false;
    }

    void FixedUpdate()
    {
        if (HUD.Instance.terminal.HasSelectedActor() && !CanReach(HUD.Instance.terminal.selectedActor.gameObject))
            HUD.Instance.terminal.ClearActorSelection();
    }
    #endregion
}