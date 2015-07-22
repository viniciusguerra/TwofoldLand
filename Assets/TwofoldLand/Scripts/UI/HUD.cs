using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class HUD : Singleton<HUD>
{
	#region Properties
    public Terminal terminal;
    public Log log;
    public Notes notes;
    public Codex codex;
    public Storage storage;
    public InfoPanel infoPanel;
    public BinaryConversion binaryConversion;

    private Slider healthBar;
    private Slider staminaBar;
    private Text auraText;
	#endregion

	#region Methods

	#endregion

    #region UI Methods
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
        auraText = GameObject.Find("AuraPanel").GetComponentInChildren<Text>();
	}

	void Update()
	{
	
	}
	#endregion
}
