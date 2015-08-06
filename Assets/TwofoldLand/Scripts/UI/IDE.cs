using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class IDE : UIWindow
{
    #region Properties
    public InputField spellInputField;
    public Text spellCostText;
    public InputField spellTitleInputField;

    public Button compileButton;
    public float compileButtonFadeTime = 0.2f;
    public float compilerEnabledAlpha = 1;
    public float compilerDisabledAlpha = 0.2f;

    public Spell currentSpell;

    public List<Spell> spellList;

    public Vector3 cameraOffsetWhenOpen;

    private string auraCostLabel = " Au to Compile";
    #endregion

    #region Methods
    //Event Triggered Methods
    public void CycleSpells()
    {
        if (spellList.Count == 0)
            return;

        if (spellList.IndexOf(currentSpell, 0) == spellList.Count - 1)
            SetCurrentSpell(spellList[0]);
        else
            SetCurrentSpell(spellList[spellList.IndexOf(currentSpell, 0) + 1]);
    }

    public void DeleteCurrentSpell()
    {
        spellList.Remove(currentSpell);

        ClearDisplay();
    }

    public void SetCurrentSpell(Spell s)
    {
        currentSpell = s;

        spellTitleInputField.interactable = true;
        spellTitleInputField.text = s.SpellTitle;
        spellTitleInputField.MoveTextEnd(false);

        spellInputField.text = s.SpellWords != null? s.SpellWords : string.Empty;
        spellInputField.MoveTextStart(false);
        spellInputField.interactable = true;        
    }

    public void UpdateSpell(string words)
    {
        if (currentSpell == null)
            return;

        currentSpell.SpellWords = words;

        spellCostText.text = currentSpell.AuraCost.ToString() + auraCostLabel;

        SetCompileButton();
    }

    public void UpdateSpellTitle(string title)
    {
        currentSpell.SpellTitle = title;
        spellTitleInputField.text = currentSpell.SpellTitle;
    }

    public void NewSpell()
    {
        Spell s = new Spell();
        spellList.Add(s);
        s.SpellTitle = "NewSpell";

        ClearDisplay();

        SetCurrentSpell(s);
    }

    public void CompileSpell()
    {
        HUD.Instance.codex.AddSpell(currentSpell);
        Ricci.Instance.SpendAura(currentSpell.AuraCost);

        spellList.Remove(currentSpell);

        ClearDisplay();
    }

    public void ListSpells()
    {
        //TODO list spells
    }

    //UI Methods
    public void SetCompileButton()
    {
        if (Ricci.Instance.IsCompilerAvailable && currentSpell != null && currentSpell.commands != null && currentSpell.commands.Length > 0 && Ricci.Instance.Aura >= currentSpell.AuraCost)
        {
            compileButton.interactable = true;           
        }
        else
        {
            compileButton.interactable = false;
        }

        compileButton.targetGraphic.color = new Color(compileButton.targetGraphic.color.r, compileButton.targetGraphic.color.g, compileButton.targetGraphic.color.b, compileButton.interactable ? compilerEnabledAlpha : compilerDisabledAlpha);
    }

    public override void Show()
    {
        base.Show();

        ClearDisplay();

        MainCamera.Instance.AddOffset(cameraOffsetWhenOpen);
    }

    public override void Hide()
    {
        base.Hide();        

        MainCamera.Instance.Center();
    }

    private void ClearDisplay()
    {
        currentSpell = null;

        spellTitleInputField.interactable = false;
        spellTitleInputField.text = "Select a Spell";
        spellTitleInputField.MoveTextEnd(false);

        spellInputField.text = string.Empty;
        spellInputField.MoveTextStart(false);

        spellInputField.interactable = false;

        spellCostText.text = string.Empty;

        SetCompileButton();
    }    
    #endregion

    #region MonoBehaviour
    public override void Start()
    {
        base.Start();
    }

    void Update()
    {

    }
    #endregion
}
