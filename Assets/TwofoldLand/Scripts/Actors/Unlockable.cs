using UnityEngine;
using System.Collections;
using System;

public class Unlockable : Actor, IUnlockable
{
    public string binaryKey;
    private bool open;
    public bool unlocked;

    private bool alreadyOpened;
    public int auraToSpawn;

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

    private Animator lidAnimator;

    public void Unlock()
    {
        HUD.Instance.binaryConversion.Create((IUnlockable)this);        
    }

    private void DisplayLockedFeedback()
    {
        HUD.Instance.log.Push(name + " locked");
        lidAnimator.SetTrigger("toggle");
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
                lidAnimator.SetBool("unlocked", unlocked);
                lidAnimator.SetBool("open", true);

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
            lidAnimator.SetTrigger("toggle");

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
        while (!lidAnimator.GetCurrentAnimatorStateInfo(0).IsName("ChestLidOpenAnimation"))
            yield return null;

        while (lidAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            yield return null;

        GameObject releasedItem = SceneManager.Instance.SpawnAura(auraToSpawn, transform.position + new Vector3(0, 0.6f, 0));

        Vector3 force = transform.TransformDirection(new Vector3(0, 300, 120));
        releasedItem.GetComponent<Rigidbody>().AddForce(force);

        alreadyOpened = true;
    }

    public override void Start()
    {
        base.Start();

        lidAnimator = GetComponentInChildren<Animator>();
        
        lidAnimator.SetBool("unlocked", unlocked);
        lidAnimator.SetBool("open", !open);

        alreadyOpened = false;
    }
}
