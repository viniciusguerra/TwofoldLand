using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;

public class InterfaceInfo : MonoBehaviour
{
    public GameObject propertyPrefab;
    public Transform propertyArea;

    public Text interfaceTitle;
    public Button goToDefinitionButton;

    private List<GameObject> propertyObjectList;
    private PropertyInfo[] interfaceProperties;
    private GameObject auxiliaryProperty;

    public void Initialize(Transform parentList, Type interfaceType)
    {
        transform.SetParent(parentList.transform, false);
        string currentName = interfaceType.Name;

        interfaceTitle.text = currentName;

        goToDefinitionButton.onClick.AddListener(() => HUD.Instance.codex.DisplayInterface(currentName));

        interfaceProperties = interfaceType.GetProperties();

        foreach (PropertyInfo interfaceProperty in interfaceProperties)
        {
            auxiliaryProperty = Instantiate(propertyPrefab);
            auxiliaryProperty.transform.SetParent(propertyArea, false);
            auxiliaryProperty.transform.FindChild("Label").GetComponent<Text>().text = interfaceProperty.Name;

            propertyObjectList.Add(auxiliaryProperty);
        }
    }

    public void Awake()
    {
        propertyObjectList = new List<GameObject>();
    }

    public void FixedUpdate()
    {
        if (interfaceProperties != null)
        {
            foreach (PropertyInfo interfaceProperty in interfaceProperties)
            {
                try
                {
                    auxiliaryProperty = propertyObjectList.Find(x => x.transform.FindChild("Label").GetComponent<Text>().text == interfaceProperty.Name);
                    auxiliaryProperty.transform.FindChild("Value").GetComponent<Text>().text = interfaceProperty.GetValue(HUD.Instance.terminal.selectedActor, null).ToString();
                }
                catch(Exception e)
                {
                    break;
                }
            }
        }
    }
}
