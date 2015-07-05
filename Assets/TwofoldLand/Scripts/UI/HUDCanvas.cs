using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class HUDCanvas : SingletonMonoBehaviour<HUDCanvas>
{
	#region Properties
    private Terminal terminal;
    private FeedbackUI feedbackUI;
	#endregion

	#region Methods

	#endregion

	#region MonoBehaviour
	void Start()
	{
        terminal = GetComponentInChildren<Terminal>();
        feedbackUI = GetComponentInChildren<FeedbackUI>();
	}

	void Update()
	{
	
	}
	#endregion
}
