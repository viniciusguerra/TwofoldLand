using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class InterfaceButtonList : InterfaceList
{
    #region Properties
    private List<Button> interfaceButtonList;

    protected override GameObject[] InterfaceButtonArray
    {
        get
        {
            Button[] buttonArray = interfaceButtonList.ToArray();

            GameObject[] goArray = new GameObject[buttonArray.Length];

            for (int i = 0; i < goArray.Length; i++)
            {
                goArray[i] = buttonArray[i].gameObject;
            }

            return goArray;
        }
    }
    #endregion

    #region Methods
    public override void DisplayInterfaces(string[] interfaceArray)
    {
        ClearInterfaces();

        foreach (string interfaceName in interfaceArray)
        {
            GameObject interfaceButton = GameObject.Instantiate(interfaceButtonPrefab);

            Button button = interfaceButton.GetComponent<Button>();

            interfaceButton.GetComponentInChildren<Text>().text = interfaceName;
            interfaceButton.transform.SetParent(listPanel.transform, false);

            //Adds click listener to Interface Button. Calls DisplayInterface sending its own name
            string currentName = interfaceName;
            button.onClick.AddListener(() => HUD.Instance.codex.DisplayInterface(currentName));

            interfaceButtonList.Add(button);
        }
    }

    protected override void ClearInterfaces()
    {
        foreach (Button b in interfaceButtonList)
        {
            Destroy(b.gameObject);
        }

        interfaceButtonList.Clear();
    }
    #endregion

    #region MonoBehaviour
    public override void Awake()
    {
        base.Awake();

        interfaceButtonList = new List<Button>();        
    }
    #endregion
}