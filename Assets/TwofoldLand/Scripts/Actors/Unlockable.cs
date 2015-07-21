using UnityEngine;
using System.Collections;
using System;

public class Unlockable : Actor, IUnlockable
{
    public string binaryKey;
    private bool open;
    public bool unlocked;

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

    }

    private void DisplayLockedFeedback()
    {
        HUD.Instance.log.Push("Chest Locked");
        lidAnimator.SetTrigger("toggle");
    }

    public void Unlock(object decimalKey)
    {
        if (unlocked)
        {
            Debug.Log("IUnlockable already unlocked");
        }
        else
        {
            if (Convert.ToString(Int16.Parse((string)decimalKey), 2) == binaryKey)
            {
                unlocked = true;
                lidAnimator.SetBool("unlocked", unlocked);
                lidAnimator.SetBool("open", true);
                lidAnimator.SetTrigger("toggle");
                open = true;

                HUD.Instance.log.Push("Chest unlocked");
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
        }
        else
        {
            DisplayLockedFeedback();
        }
    }

    public override void Start()
    {
        base.Start();

        lidAnimator = GetComponentInChildren<Animator>();
        
        lidAnimator.SetBool("unlocked", unlocked);
        lidAnimator.SetBool("open", !open);
    }
}
