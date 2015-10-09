using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class InterfaceInfoList : InterfaceList
{
    #region Properties
    public GameObject interfaceInfoPrefab;
    private List<GameObject> interfaceInfoList;

    protected override GameObject[] InterfaceButtonArray
    {
        get
        {
            GameObject[] buttonArray = interfaceInfoList.ToArray();

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
    public override void DisplayInterfaces(Type[] interfaceArray)
    {
        foreach (Type interfaceType in interfaceArray)
        {
            /*
            Old implementation
            GameObject interfaceButton = GameObject.Instantiate(interfaceButtonPrefab);

            Button button = interfaceButton.GetComponent<Button>();

            interfaceButton.GetComponentInChildren<Text>().text = interfaceName;
            interfaceButton.transform.SetParent(listPanel.transform, false);

            //Adds click listener to Interface Button. Calls DisplayInterface sending its own name
            string currentName = interfaceName;
            button.onClick.AddListener(() => HUD.Instance.codex.DisplayInterface(currentName));

            interfaceInfoList.Add(button);
            */

            InterfaceInfo interfaceInfo = Instantiate(interfaceInfoPrefab).GetComponent<InterfaceInfo>();

            interfaceInfo.Initialize(listPanel.transform, interfaceType);

            interfaceInfoList.Add(interfaceInfo.gameObject);
        }
    }

    public override void ClearInterfaces()
    {
        foreach (GameObject go in interfaceInfoList)
        {
            Destroy(go);
        }

        interfaceInfoList.Clear();
    }
    #endregion

    #region MonoBehaviour
    public override void Awake()
    {
        base.Awake();

        interfaceInfoList = new List<GameObject>();        
    }
    #endregion
}