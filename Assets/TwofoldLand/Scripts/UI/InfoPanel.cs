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
    public InterfaceButtonList interfaceList;
	#endregion

	#region Methods
    public void Show(string actorName, string[] interfaces)
    {
        base.Show();

        actorNameText.text = actorName;

        ToggleHealthBar(false);        

        interfaceList.DisplayInterfaces(interfaces);
    }

    public void Show(string actorName, string[] interfaces, IDamageable damageable)
    {
        base.Show();

        actorNameText.text = actorName;

        ToggleHealthBar(true);

        StartCoroutine(UpdateHealthBar(damageable));

        interfaceList.DisplayInterfaces(interfaces);
    }

    public override void Hide()
    {
        base.Hide();

        StopCoroutine("UpdateHealthBar");
    }

    private void ToggleHealthBar(bool toggle)
    {
        //Calculate interface panel height

        if (toggle)
        {            
            
        }
        else
        {
            
        }

        healthBar.gameObject.SetActive(toggle);
    }

    private IEnumerator UpdateHealthBar(IDamageable damageable)
    {
        while(true)
        {
            healthBar.value = damageable.Health;
        }
    }
	#endregion

	#region MonoBehaviour

	#endregion
}
