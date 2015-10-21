using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ItemContainer : Collectable, IPointerClickHandler
{

    public override bool CollectOnTouch
    {
        get
        {
            return false;
        }
    }

    public ItemData itemData;

    private HoveringItem hoveringItem;

    public override void Absorb()
    {
        hoveringItem.JumpAndFade();

        HUD.Instance.storage.AcquireItem(itemData);
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
        if (Player.Instance.IsInSelectionRange(transform.position))
            Absorb();
    }

    new void Update()
    {
        base.Update();
    }
}
