using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class InfoPanel : UIWindow
{
	#region Properties
    public Text actorNameText;
    public Slider healthBar;
    public Text interfaceListText;
	#endregion

	#region Methods
    public void Show(bool showHealthBar, string actorName, string[] interfaces)
    {
        healthBar.gameObject.SetActive(showHealthBar);

        interfaceListText.text = string.Empty;

        //TODO Add height to cover health bar space if it is not present

        foreach(string s in interfaces)
        {
            interfaceListText.text += s + "\n";
        }

        Toggle(true);
    }

    public void Hide()
    {
        Toggle(false);
    }
	#endregion

	#region MonoBehaviour
	void Start()
	{
        //healthBar = GetComponentInChildren<Slider>();
        //interfaceListText = transform.FindChild("InterfaceList").GetComponent<Text>();
	}

	void Update()
	{

	}
	#endregion
}
