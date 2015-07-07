using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class HUD : SingletonMonoBehaviour<HUD>
{
	#region Properties
    private Terminal terminal;
    private Log log;
    public Notes notes;
    public Codex codex;
    public Storage storage;
    public InfoPanel infoPanel;

    private Slider healthBar;
    private Slider staminaBar;
    private Text auraText;    

    public static Terminal Terminal
    {
        get
        {
            return Instance.terminal;
        }
    }

    public static Log Log
    {
        get
        {
            return Instance.log;
        }
    }

    public static Notes Notes
    {
        get
        {
            return Instance.notes;
        }
    }

    public static Storage Storage
    {
        get
        {
            return Instance.storage;
        }
    }

    public static Codex Codex
    {
        get
        {
            return Instance.codex;
        }
    }

    public static InfoPanel InfoPanel
    {
        get
        {
            return Instance.infoPanel;
        }
    }

    public static Slider HealthBar
    {
        get
        {
            return Instance.healthBar;
        }
    }

    public static Slider StaminaBar
    {
        get
        {
            return Instance.staminaBar;
        }
    }

    public static Text Aura
    {
        get
        {
            return Instance.auraText;
        }
    }
	#endregion

	#region Methods

	#endregion

    #region UI Methods
    public void ShowCodex()
    {

    }

    public void ShowMemory()
    {

    }

    public void ShowNotes()
    {

    }
    #endregion

    #region MonoBehaviour
    void Start()
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
