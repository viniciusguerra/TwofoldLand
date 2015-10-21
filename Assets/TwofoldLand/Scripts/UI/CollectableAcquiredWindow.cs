using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class CollectableAcquiredWindow : UIWindow
{
    [Space(20)]
    public Text messageLabelText;
    public Text collectableNameText;    
    public Button goToDefinitionButton;

    [Header("Fill")]
    public Image fillImage;
    public float fillTime = 0.333f;
    public float fillDelay = 0.2f;

    private string skillAcquiredMessage = "New Skill Acquired!";
    private string itemAcquiredMessage = "New Item Acquired!";

    public void Show(Skill skill)
    {
        base.Show();

        fillImage.fillAmount = 0;

        iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1, "delay", fillDelay, "time", fillTime, "onupdate", "UpdateFill"));

        collectableNameText.text = skill.GetInterfaceType().Name;
        messageLabelText.text = skillAcquiredMessage;

        goToDefinitionButton.gameObject.SetActive(true);
        goToDefinitionButton.onClick.RemoveAllListeners();

        goToDefinitionButton.onClick.AddListener(() => HUD.Instance.codex.DisplayInterface(skill.GetInterfaceType().Name));
        goToDefinitionButton.onClick.AddListener(() => this.Hide());
    }

    public void Show(Item item)
    {
        base.Show();

        fillImage.fillAmount = 0;

        iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1, "delay", fillDelay, "time", fillTime, "onupdate", "UpdateFill"));

        collectableNameText.text = item.ItemName;
        messageLabelText.text = itemAcquiredMessage;

        goToDefinitionButton.gameObject.SetActive(false);

        //goToDefinitionButton.onClick.RemoveAllListeners();
        //goToDefinitionButton.onClick.AddListener(() => HUD.Instance.codex.DisplayInterface(interfaceType.Name));
        //goToDefinitionButton.onClick.AddListener(() => this.Hide());
    }

    public void UpdateFill(float amount)
    {
        fillImage.fillAmount = amount;
    }
}
