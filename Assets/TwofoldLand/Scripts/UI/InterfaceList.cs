using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class InterfaceList : MonoBehaviour
{
    #region Properties
    public GameObject interfaceButtonPrefab;

    protected abstract GameObject[] InterfaceButtonArray
    {
        get;
    }

    protected GameObject listPanel;

    protected float panelHeight;
    protected static float buttonHeight = -1;
    #endregion

    #region Methods
    public abstract void DisplayInterfaces(string[] interfaceArray);

    protected abstract void ClearInterfaces();

    protected void UpdatePanelHeight()
    {
        RectTransform listPanelRectTransform = listPanel.GetComponent<RectTransform>();

        Rect listPanelRect = listPanelRectTransform.rect;

        listPanelRectTransform.rect.Set(listPanelRect.xMin, listPanelRect.yMin, listPanelRect.width, Mathf.Max(panelHeight, buttonHeight * InterfaceButtonArray.Length));
    }

    protected void UpdatePositions()
    {
        for (int i = 0; i < InterfaceButtonArray.Length; i++)
        {
            InterfaceButtonArray[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -buttonHeight * i);
        }
    }
    #endregion

    #region MonoBehaviour
    public virtual void Awake()
    {
        listPanel = transform.FindChild("InterfaceListPanel").gameObject;

        panelHeight = listPanel.GetComponent<RectTransform>().rect.height;

        if (buttonHeight == -1)
            buttonHeight = interfaceButtonPrefab.GetComponent<RectTransform>().sizeDelta.y;        
    }
    #endregion
}
