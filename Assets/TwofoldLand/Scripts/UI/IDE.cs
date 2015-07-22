using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class IDE : UIWindow
{
	#region Properties

	#endregion

	#region Methods
    //UI Methods
    public override void Show()
    {
        base.Show();

        HUD.Instance.infoPanel.Hide();
        
        CameraFollow.Instance.AddOffset(new Vector3(-5, 0, 0));
    }

    public override void Hide()
    {
 	    base.Hide();

        CameraFollow.Instance.Center();
    }
	#endregion

	#region MonoBehaviour
	public override void Start()
	{
        base.Start();
	}

	void Update()
	{

	}
	#endregion
}
