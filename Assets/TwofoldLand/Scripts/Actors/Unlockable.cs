using UnityEngine;
using System.Collections;
using System;

public class Unlockable : Actor, IUnlockable, IVulnerable
{
    [SerializeField]
    private string binaryKey;
    [SerializeField]
    private bool unlocked;

    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float currentHealth;

    public float CurrentHealth
    {
        get
        {
            return maxHealth;
        }
    }

    public float MaxHealth
    {
        get
        {
            return currentHealth;
        }
    }

    public string BinaryKey
    {
        get
        {
            return binaryKey;
        }
    }

    public bool Unlocked
    {
        get
        {
            return unlocked;
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

    public int auraToSpawn;
    public SkillData[] skillsToSpawn;

    public int lockBreakThreshold;

    private bool alreadyOpened;
    private bool open;
    public Animator animator;

    private void DamageLock(object sender, AttackArgs attackArgs)
    {
        if (attackArgs.rawDamage > lockBreakThreshold)
        {
            Unlock(binaryKey);
            attackHandler = new AttackHandler(Break);
        }
    }

    private void Break(object sender, AttackArgs attackArgs)
    {
        currentHealth = Mathf.Max(0, currentHealth - attackArgs.rawDamage);
    }

    public void Unlock()
    {
        HUD.Instance.binaryConversion.Create((IUnlockable)this);        
    }

    private void DisplayLockedFeedback()
    {
        HUD.Instance.log.Push(name + " locked");
        animator.SetTrigger("toggle");
    }

    public void Unlock(object key)
    {
        if (unlocked)
        {
            HUD.Instance.log.Push(name + " already unlocked");
        }
        else
        {
            if (Convert.ToString(Int16.Parse((string)key), 2) == binaryKey)
            {
                unlocked = true;
                animator.SetBool("unlocked", unlocked);
                animator.SetBool("open", true);

                Toggle();

                HUD.Instance.log.Push(name + " unlocked");
            }
            else
            {
                DisplayLockedFeedback();
            }
        }
    }

    public void Toggle()
    {
        if (unlocked)
        {
            open = !open;
            animator.SetTrigger("toggle");

            if(!alreadyOpened)
            {
                StartCoroutine(ReleaseItemCoroutine());
            }
        }
        else
        {
            DisplayLockedFeedback();
        }
    }

    private IEnumerator ReleaseItemCoroutine()
    {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("ChestLidOpenAnimation"))
            yield return null;

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            yield return null;

        GameObject aura = SceneManager.Instance.SpawnAura(auraToSpawn, transform.position + new Vector3(0, 0.6f, 0));

        Vector3 force = transform.TransformDirection(new Vector3(UnityEngine.Random.Range(-1, 1) * 100, 300, 120));

        aura.GetComponent<Rigidbody>().AddForce(force);

        yield return new WaitForSeconds(0.2f);

        foreach (SkillData s in skillsToSpawn)
        {
            GameObject skill = SceneManager.Instance.SpawnSkill(s, transform.position + new Vector3(0, 0.6f, 0));

            force = transform.TransformDirection(new Vector3(UnityEngine.Random.Range(-1, 1) * 10, 300, 120));

            skill.GetComponent<Rigidbody>().AddForce(force);

            yield return new WaitForSeconds(0.2f);
        }        

        alreadyOpened = true;
    }

    public override void Start()
    {
        base.Start();
        
        animator.SetBool("unlocked", unlocked);
        animator.SetBool("open", !open);

        alreadyOpened = false;

        if (currentHealth == 0)
            currentHealth = maxHealth;

        attackHandler = unlocked ? new AttackHandler(Break) : new AttackHandler(DamageLock);
    }
}
