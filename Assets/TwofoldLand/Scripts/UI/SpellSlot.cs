using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class SpellSlot : MonoBehaviour
{
	#region Properties
    private Spell spell;

    public Spell Spell
    {
        get { return spell; }
    }

    public Text title;
    public Text stamina;
    public Text address;
    public Toggle toggle;
	#endregion

	#region Methods
    public void SetSpell(Spell spell)
    {
        this.spell = spell;

        title.text = spell.SpellTitle;
        stamina.text = spell.StaminaCost.ToString();
    }
	#endregion

	#region MonoBehaviour
	#endregion
}
