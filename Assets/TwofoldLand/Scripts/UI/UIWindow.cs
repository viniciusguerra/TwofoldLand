﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class UIWindow : MonoBehaviour
{
	#region Properties

	#endregion

	#region Methods
    public virtual void Toggle(bool show)
    {
        gameObject.SetActive(show);
    }
	#endregion

	#region MonoBehaviour
	void Start()
	{

	}

	void Update()
	{

	}
	#endregion
}
