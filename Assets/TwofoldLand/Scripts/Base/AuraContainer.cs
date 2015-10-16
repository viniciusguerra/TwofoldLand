using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(HoveringItem))]
public class AuraContainer : Collectable
{
    #region Properties
    public int auraAmount;

    private HoveringItem hoveringItem;
	#endregion

	#region Collectable Methods
    public override void Absorb()
    {
        hoveringItem.JumpAndFade();

        Player.Instance.CollectAura(auraAmount);
    }    

    protected override void Destroy()
    {
        Destroy(gameObject);
    }

    protected override void Idle()
    {
        hoveringItem.Rotate();
    }
    #endregion

    #region MonoBehaviour
	new void Awake()
	{
        base.Awake();

        hoveringItem = GetComponent<HoveringItem>();     
	}

	new void Update()
	{
        base.Update();        
	}
	#endregion
}
