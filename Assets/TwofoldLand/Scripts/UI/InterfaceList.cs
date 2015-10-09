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
    public abstract void DisplayInterfaces(Type[] interfaceArray);

    public abstract void ClearInterfaces();
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
