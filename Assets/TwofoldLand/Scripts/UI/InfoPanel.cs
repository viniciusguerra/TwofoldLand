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
    public InterfaceInfoList interfaceList;
	#endregion

	#region Methods
    public void Show(string actorName, Type[] interfaces)
    {
        base.Show();

        actorNameText.text = actorName;

        HideHealthBar();

        interfaceList.ClearInterfaces();

        interfaceList.DisplayInterfaces(interfaces);
    }

    public void Show(string actorName, Type[] interfaces, IVulnerable vulnerable)
    {
        base.Show();

        actorNameText.text = actorName;

        ShowHealthBar(vulnerable);

        StartCoroutine(UpdateHealthBar(vulnerable));

        interfaceList.ClearInterfaces();

        interfaceList.DisplayInterfaces(interfaces);
    }

    public override void Hide()
    {
        base.Hide();

        StopCoroutine("UpdateHealthBar");

        interfaceList.ClearInterfaces();
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
            healthBar.value = vulnerable.MaxHealth;
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
