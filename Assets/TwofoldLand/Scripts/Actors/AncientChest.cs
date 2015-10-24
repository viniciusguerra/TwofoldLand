using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class AncientChest : Entity, IUnlockable, IVulnerable
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
    public CollectableData[] collectablesToSpawn;

    public ItemData keyItemData;
    public int lockBreakThreshold;

    private bool alreadyOpened;
    private bool open;
    public Animator animator;
    //public Text codeText;

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
        HUD.Instance.binaryConversion.Show(this);        
    }

    public void Unlock(object key)
    {
        if (unlocked)
        {
            HUD.Instance.log.ShowMessage(name + " already unlocked");
        }
        else
        {
            int itemAmount;
            Item itemAtAddress = Item.GetItemFromCommandParameter(key.ToString(), out itemAmount);

            if (itemAtAddress != null && itemAtAddress.ItemData == keyItemData && itemAtAddress.Use(itemAmount))
            {
                SetUnlocked();
            }
            else
            {
                if (key.ToString().Equals(BinaryKey))
                    SetUnlocked();
                else
                {
                    OnCommandFailure(name + " locked");
                    animator.SetTrigger("toggle");
                }
            }
        }
    }

    public void SetUnlocked()
    {
        //codeText.text = BinaryKey;

        unlocked = true;
        animator.SetBool("unlocked", unlocked);
        animator.SetBool("open", true);

        Toggle();

        HUD.Instance.log.ShowMessage(name + " unlocked");
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

            OnCommandSuccess();
        }
        else
        {
            OnCommandFailure(name + " locked");
            animator.SetTrigger("toggle");
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

        foreach (CollectableData collectable in collectablesToSpawn)
        {
            GameObject collectableGameObject = SceneManager.Instance.SpawnCollectable(collectable, transform.position + new Vector3(0, 0.6f, 0));

            force = transform.TransformDirection(new Vector3(UnityEngine.Random.Range(-1, 1) * 10, 300, 120));

            collectableGameObject.GetComponent<Rigidbody>().AddForce(force);

            yield return new WaitForSeconds(0.2f);
        }        

        alreadyOpened = true;
    }

    public void Awake()
    {
        animator.SetBool("unlocked", unlocked);
        animator.SetBool("open", !open);

        alreadyOpened = false;

        if (currentHealth == 0)
            currentHealth = maxHealth;

        attackHandler = unlocked ? new AttackHandler(Break) : new AttackHandler(DamageLock);

        //codeText.text = Convert.ToInt32(BinaryKey, 2).ToString();
    }
}
