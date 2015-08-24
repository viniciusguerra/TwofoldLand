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

        HideHealthBar();      

        interfaceList.DisplayInterfaces(interfaces);
    }

    public void Show(string actorName, string[] interfaces, IVulnerable vulnerable)
    {
        base.Show();

        actorNameText.text = actorName;

        ShowHealthBar(vulnerable);

        StartCoroutine(UpdateHealthBar(vulnerable));

        interfaceList.DisplayInterfaces(interfaces);
    }

    public override void Hide()
    {
        base.Hide();

        StopCoroutine("UpdateHealthBar");
    }

    private void ShowHealthBar(IVulnerable vulnerable)
    {
        healthBar.transform.parent.gameObject.SetActive(true);
        StartCoroutine(UpdateHealthBar(vulnerable));
    }

    private void HideHealthBar()
    {
        healthBar.transform.parent.gameObject.SetActive(false);
        StopCoroutine("UpdateHealthBar");
    }

    private IEnumerator UpdateHealthBar(IVulnerable vulnerable)
    {
        healthBar.maxValue = vulnerable.MaxHealth;

        while(true)
        {
            healthBar.value = vulnerable.CurrentHealth;
            yield return null;
        }
    }
	#endregion

	#region MonoBehaviour
    void FixedUpdate()
    {
        if (HUD.Instance.ide.IsVisible)
            Hide();
    }
	#endregion
}
