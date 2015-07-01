﻿using UnityEngine;
using System.Collections;
using System;

public class Unlockable : Actor, IUnlockable
{
    public string binaryKey;
    public bool unlocked;
    private bool open;

    private Animator lidAnimator;

    public void Unlock()
    {

    }

    private void DisplayLockedFeedback()
    {
        FeedbackUI.Instance.Log("Chest Locked");
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

                FeedbackUI.Instance.Log("Chest unlocked");
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
