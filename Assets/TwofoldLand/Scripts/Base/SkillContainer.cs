using UnityEngine;
using System.Collections;
using System;

public class SkillContainer : Collectable
{
    public SkillData skillData;

    private HoveringItem hoveringItem;

    public override void Absorb()
    {
        hoveringItem.JumpAndFade();

        Ricci.Instance.AddSkill(skillData);
    }

    protected override void Destroy()
    {
        Destroy(gameObject);
    }

    protected override void Idle()
    {
        hoveringItem.Rotate();
    }

    new void Awake()
    {
        base.Awake();

        hoveringItem = GetComponent<HoveringItem>();
    }

    new void Update()
    {
        base.Update();
    }
}
