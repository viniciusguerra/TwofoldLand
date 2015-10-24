using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.UI;
using System;

public class Actor : MonoBehaviour, IPointerClickHandler
{
    private int originalLayer;

    [SerializeField]
    private Entity entity;
    public Entity Entity
    {
        get
        {
            if (entity == null)
                GetComponent<Entity>();

            return entity;
        }
    }

    public void DisplayInterfaces()
    {
        bool showHealthBar = Entity.KnownInterfaceList.Contains(typeof(IVulnerable));

        if (showHealthBar)
            HUD.Instance.infoPanel.Show(name, Entity.KnownInterfaceList.ToArray(), Entity as IVulnerable);
        else
            HUD.Instance.infoPanel.Show(name, Entity.KnownInterfaceList.ToArray());
    }

    public void HideInterfaces()
    {
        HUD.Instance.infoPanel.Hide();
    }

    public virtual void SetSelected()
    {
        gameObject.SetLayerRecursively(MainCamera.outlineLayer);

        HUD.Instance.terminal.SetSelectedActor(this);

        DisplayInterfaces();
    }

    public virtual void Deselect()
    {
        gameObject.SetLayerRecursively(originalLayer);

        HideInterfaces();
    }

    public virtual void OnPointerClick(PointerEventData data)
    {
        if (data.pointerId == -1 && Player.Instance.CanReach(gameObject))
        {
            SetSelected();
        }
    }

    public virtual void Awake()
    {
        if (entity == null)
            GetComponent<Entity>();

        originalLayer = gameObject.layer;
    }
}
