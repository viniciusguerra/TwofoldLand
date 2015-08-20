using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class Storage : UIWindow
{
	#region Properties
    public SpellSlot currentSelectedSpellSlot;
    public List<SpellSlot> spellSlotList;
    public float offsetTime = 0.1f;

    public ToggleGroup loadedSpellsToggleGroup;
    private RectTransform rt;
	#endregion

	#region Methods
    public override void Toggle()
    {
        base.Toggle();

        if (isVisible)
        {
            MainCamera.Instance.SetOffset(MainCameraOffsetDirection.Right);
        }
        else
        {
            if(currentSelectedSpellSlot != null)
                CancelSlotSelection();

            MainCamera.Instance.SetOffset(MainCameraOffsetDirection.Reset);
        }
    }

    public override void Show()
    {
        base.Show();

        MainCamera.Instance.SetOffset(MainCameraOffsetDirection.Right);
    }

    public override void Hide()
    {
        base.Hide();

        if (currentSelectedSpellSlot != null)
            CancelSlotSelection();

        MainCamera.Instance.SetOffset(MainCameraOffsetDirection.Reset);
    }

    public Spell GetSpellFromSlot(int slot)
    {
        return spellSlotList[slot].Spell;
    }

    //UI Methods
    public Spell GetSpellFromAddress(string address)
    {
        foreach(SpellSlot s in spellSlotList)
        {
            if (s.address.text.Equals(address))
                return s.Spell;
        }

        return null;
    }

    public void SetSlotSelection(SpellSlot spellSlot)
    {
        if (currentSelectedSpellSlot == spellSlot)
        {
            if(!spellSlot.toggle.isOn && !loadedSpellsToggleGroup.AnyTogglesOn())
                CancelSlotSelection();
        }
        else
        {
            currentSelectedSpellSlot = spellSlot;

            if (!HUD.Instance.codex.IsVisible)
            {
                OffsetShowCodex();
                HUD.Instance.codex.Show();
                HUD.Instance.codex.DisplaySpellArea();
            }
        }        
    }

    public void CancelSlotSelection()
    {
        currentSelectedSpellSlot = null;

        loadedSpellsToggleGroup.SetAllTogglesOff();
        OffsetHideCodex();
        HUD.Instance.codex.Hide();
    }

    public void OffsetShowCodex()
    {
        string name = GetInstanceID() + "Offset";

        if (rt.anchoredPosition.x == 0)
        {
            iTween.StopByName(name);
            float codexWidth = RectTransformUtility.CalculateRelativeRectTransformBounds(HUD.Instance.codex.transform).size.x;
            iTween.ValueTo(gameObject, iTween.Hash("name", name, "from", rt.anchoredPosition.x, "to", -codexWidth, "onupdate", "UpdateOffset", "time", offsetTime));
        }
        else
            return;
    }

    public void OffsetHideCodex()
    {
        string name = GetInstanceID() + "Offset";

        iTween.StopByName(name);

        if (rt.anchoredPosition.x < 0)
        {
            iTween.ValueTo(gameObject, iTween.Hash("name", name, "from", rt.anchoredPosition.x, "to", 0, "onupdate", "UpdateOffset", "time", offsetTime));
        }
        else
            return;
    }

    public void UpdateOffset(float value)
    {
        rt.anchoredPosition = new Vector2(value, rt.anchoredPosition.y);
    }

    public void SelectSpell(Spell spell)
    {
        if(currentSelectedSpellSlot != null)
        {
            currentSelectedSpellSlot.SetSpell(spell);
            CancelSlotSelection();
        }
    }
	#endregion

	#region MonoBehaviour
    void Awake()
    {
        rt = GetComponent<RectTransform>();
    }
	#endregion
}
