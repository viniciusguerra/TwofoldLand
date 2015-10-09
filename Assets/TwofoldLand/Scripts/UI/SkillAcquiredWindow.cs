using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class SkillAcquiredWindow : UIWindow
{
    [Space(20)]
    public Text skillTitle;    
    public Button goToDefinitionButton;

    [Header("Fill")]
    public Image fillImage;
    public float fillTime = 0.333f;
    public float fillDelay = 0.2f;

    public void Show(Type interfaceType)
    {
        base.Show();

        fillImage.fillAmount = 0;

        iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1, "delay", fillDelay, "time", fillTime, "onupdate", "UpdateFill"));

        skillTitle.text = interfaceType.Name;

        goToDefinitionButton.onClick.RemoveAllListeners();

        goToDefinitionButton.onClick.AddListener(() => HUD.Instance.codex.DisplayInterface(interfaceType.Name));
        goToDefinitionButton.onClick.AddListener(() => this.Hide());
    }

    public void UpdateFill(float amount)
    {
        fillImage.fillAmount = amount;
    }
}
