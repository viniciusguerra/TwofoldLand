using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class SkillContainer : Collectable, IPointerClickHandler
{
    public override bool CollectOnTouch
    {
        get
        {
            return false;
        }
    }

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

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if(Ricci.Instance.IsInSelectionRange(transform.position))
            Absorb();
    }

    new void Update()
    {
        base.Update();
    }
}
