﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class InterfaceToggleList : InterfaceList
{
    #region Properties
    private ToggleGroup toggleGroup;

    private List<Toggle> interfaceToggleList;

    protected override GameObject[] InterfaceButtonArray
    {
        get
        {
            Toggle[] toggleArray = interfaceToggleList.ToArray();

            GameObject[] goArray = new GameObject[toggleArray.Length];

            for (int i = 0; i < goArray.Length; i++)
            {
                goArray[i] = toggleArray[i].gameObject;
            }

            return goArray;
        }
    }
    #endregion

    #region Methods
    public override void DisplayInterfaces(Type[] interfaceArray)
    {
        ClearInterfaces();

        foreach (Type interfaceType in interfaceArray)
        {
            GameObject interfaceButton = GameObject.Instantiate(interfaceButtonPrefab);
            string currentName = interfaceType.Name;

            Toggle toggle = interfaceButton.GetComponent<Toggle>();
            toggle.group = toggleGroup;

            interfaceButton.GetComponentInChildren<Text>().text = currentName;
            interfaceButton.transform.SetParent(listPanel.transform, false);

            //Adds click listener to Interface Toggle. Calls DisplayInterface sending its own name
            
            toggle.onValueChanged.AddListener((bool value) => HUD.Instance.codex.DisplayInterface(value, currentName));

            interfaceToggleList.Add(toggle);
        }
    }

    public override void ClearInterfaces()
    {
        foreach (Toggle t in interfaceToggleList)
        {
            Destroy(t.gameObject);
        }

        interfaceToggleList.Clear();
    }
    #endregion

    #region MonoBehaviour
    public override void Awake()
    {
        base.Awake();

        toggleGroup = GetComponent<ToggleGroup>();
        interfaceToggleList = new List<Toggle>(); 
    }
    #endregion
}
