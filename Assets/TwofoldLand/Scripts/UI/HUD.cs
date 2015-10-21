using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class HUD : Singleton<HUD>
{
    #region Properties
    [Header("Bottom Left Area")]
    public RectTransform selectionArea;
    public UIWindow selectionPanel;
    public float bottomLeftRectResizeTime = 0.2f;
    public float selectionPanelHeight = 60;
    public float selectionAreaWidth = 25;
    public float selectionPanelWidth = 90;
    [Space(20)]
    public Terminal terminal;
    public IDE ide;
    public InfoPanel infoPanel;    

    [Header("Windows")]
    public Notes notes;
    public Codex codex;
    public Storage storage;
    public CollectableAcquiredWindow collectableAcquiredWindow;

    public Log log;
    public BinaryConversion binaryConversion;

    [Header("Top Left Bars")]
    public float barUpdateTime = 0.1f;
    public iTween.EaseType barUpdateEaseType = iTween.EaseType.easeOutCubic;

    [Space(20)]
    public Color healthNormalColor;
    public Color healthRegenColor;
    public Color healthLossColor;

    [Space(20)]
    public Color staminaNormalColor;
    public Color staminaRegenColor;
    public Color staminaLossColor;

    private Slider healthBar;
    private Slider staminaBar;
    private Graphic healthBarFill;
    private Graphic staminaBarFill;

    private Text auraText;
    private RectTransform bottomLeftRT;
    private RectTransform terminalRT;
    private RectTransform ideRT;
    #endregion

    #region Methods

    #endregion

    #region UI Methods
    //Bottom Left Selection Panel
    public void ShowBottomLeftSelectionPanel()
    {
        if (infoPanel.IsVisible)
            infoPanel.Hide();

        selectionPanel.Show();

        bottomLeftRT.sizeDelta = new Vector2(bottomLeftRT.sizeDelta.x, 0);

        iTween.StopByName(GetInstanceID() + "BottomLeftSelectionArea");
        iTween.ValueTo(gameObject, iTween.Hash("name", GetInstanceID() + "BottomLeftSelectionArea", "onupdate", "UpdateBottomLeftSelectionAreaWidth", "from", selectionArea.sizeDelta.x, "to", selectionPanelWidth, "time", bottomLeftRectResizeTime, "oncomplete", "ShowBottomLeftSelectionPanel_Part2"));
    }

    private void ShowBottomLeftSelectionPanel_Part2()
    {
        iTween.StopByName(GetInstanceID() + "BottomLeftSelectionPanel");
        iTween.ValueTo(gameObject, iTween.Hash("name", GetInstanceID() + "BottomLeftSelectionPanel", "onupdate", "UpdateBottomLeftSelectionPanelHeight", "from", bottomLeftRT.sizeDelta.y, "to", selectionPanelHeight, "time", bottomLeftRectResizeTime));
    }

    public void HideBottomLeftSelectionPanel()
    {
        iTween.StopByName(GetInstanceID() + "BottomLeftSelectionPanel");
        iTween.ValueTo(gameObject, iTween.Hash("name", GetInstanceID() + "BottomLeftSelectionPanel", "onupdate", "UpdateBottomLeftSelectionPanelHeight", "from", bottomLeftRT.sizeDelta.y, "to", 0, "time", bottomLeftRectResizeTime, "oncomplete", "HideBottomLeftSelectionPanel_Part2"));
    }

    private void HideBottomLeftSelectionPanel_Part2()
    {
        iTween.StopByName(GetInstanceID() + "BottomLeftSelectionArea");
        iTween.ValueTo(gameObject, iTween.Hash("name", GetInstanceID() + "BottomLeftSelectionArea", "onupdate", "UpdateBottomLeftSelectionAreaWidth", "from", selectionArea.sizeDelta.x, "to", selectionAreaWidth, "time", bottomLeftRectResizeTime, "oncompletetarget", selectionPanel.gameObject, "oncomplete", "Hide"));

        if (terminal.selectedActor != null)
            infoPanel.Show();
    }

    private void UpdateBottomLeftSelectionPanelHeight(float height)
    {
        bottomLeftRT.sizeDelta = new Vector2(bottomLeftRT.sizeDelta.x, height);
    }

    private void UpdateBottomLeftSelectionAreaWidth(float width)
    {
        selectionArea.sizeDelta = new Vector2(width, selectionArea.sizeDelta.y);
        terminalRT.anchoredPosition = new Vector2(width, terminalRT.anchoredPosition.y);
        ideRT.anchoredPosition = new Vector2(width, ideRT.anchoredPosition.y);
        //terminalRT.rect.Set(selectionAreaWidth + width, terminalRT.anchoredPosition.y, terminalRT.rect.width, terminalRT.rect.height);
        //ideRT.rect.Set(selectionAreaWidth + width, ideRT.anchoredPosition.y, ideRT.rect.width, ideRT.rect.height);
    }

    //Top Left Bars
    public void SetHealthBarValue(float value)
    {
        string valueTweenName = healthBar.name + "ValueChange";
        string colorTweenName = healthBar.name + "ColorChange";
        iTween.StopByName(valueTweenName);
        iTween.StopByName(colorTweenName);

        UpdateHealthBarColor(value < healthBar.value ? healthLossColor : healthRegenColor);
        iTween.ValueTo(gameObject, iTween.Hash("name", colorTweenName, "from", healthBarFill.color, "to", healthNormalColor, "onupdate", "UpdateHealthBarColor", "time", barUpdateTime * 5, "easetype", barUpdateEaseType));

        iTween.ValueTo(gameObject, iTween.Hash("name", valueTweenName, "from", healthBar.value, "to", value, "onupdate", "UpdateHealthBarValue", "time", barUpdateTime, "easetype", barUpdateEaseType));
    }

    public void SetStaminaBarValue(float value)
    {
        string valueTweenName = staminaBar.name + "ValueChange";
        string colorTweenName = staminaBar.name + "ColorChange";
        iTween.StopByName(valueTweenName);
        iTween.StopByName(colorTweenName);

        UpdateStaminaBarColor(value < staminaBar.value ? staminaLossColor : staminaRegenColor);
        iTween.ValueTo(gameObject, iTween.Hash("name", colorTweenName, "from", staminaBarFill.color, "to", staminaNormalColor, "onupdate", "UpdateStaminaBarColor", "time", barUpdateTime * 5, "easetype", barUpdateEaseType));

        iTween.ValueTo(gameObject, iTween.Hash("name", valueTweenName, "from", staminaBar.value, "to", value, "onupdate", "UpdateStaminaBarValue", "time", barUpdateTime, "easetype", barUpdateEaseType));
    }

    public void UpdateHealthBarValue(float value)
    {
        healthBar.value = value;
    }

    public void UpdateHealthBarColor(Color value)
    {
        healthBarFill.color = value;
    }

    public void UpdateStaminaBarValue(float value)
    {
        staminaBar.value = value;
    }

    public void UpdateStaminaBarColor(Color value)
    {
        staminaBarFill.color = value;
    }

    public void SetMaxHealth(float value)
    {
        healthBar.maxValue = value;
    }

    public void SetMaxStamina(float value)
    {
        staminaBar.maxValue = value;
    }

    //Aura
    public void UpdateAuraUI(float amount)
    {
        auraText.text = amount + " Au";
    }
    #endregion

    #region MonoBehaviour
    void Awake()
    {
        terminal = GetComponentInChildren<Terminal>();
        log = GetComponentInChildren<Log>();
        //storage = GetComponentInChildren<Storage>();
        //notes = GetComponentInChildren<Notes>();
        //codex = GetComponentInChildren<Codex>();
        //infoPanel = GetComponentInChildren<InfoPanel>();
        healthBar = GameObject.Find("HealthBar").GetComponentInChildren<Slider>();
        staminaBar = GameObject.Find("StaminaBar").GetComponentInChildren<Slider>();
        healthBarFill = healthBar.fillRect.GetComponent<Graphic>();
        staminaBarFill = staminaBar.fillRect.GetComponent<Graphic>();
        auraText = GameObject.Find("AuraPanel").GetComponentInChildren<Text>();
        bottomLeftRT = selectionPanel.GetComponent<RectTransform>();
        terminalRT = terminal.GetComponent<RectTransform>();
        ideRT = ide.GetComponent<RectTransform>();
    }

    void Update()
    {

    }
    #endregion
}
