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
    private List<PropertyInfo> interfaceProperties;
    private GameObject auxiliaryProperty;

    public void Initialize(Transform parentList, Type interfaceType)
    {
        transform.SetParent(parentList.transform, false);
        string currentName = interfaceType.Name;

        interfaceTitle.text = currentName;

        goToDefinitionButton.onClick.AddListener(() => HUD.Instance.codex.DisplayInterface(currentName));

        interfaceProperties.AddRange(interfaceType.GetProperties());

        foreach (PropertyInfo interfaceProperty in interfaceProperties)
        {
            CodexPropertyAttribute descriptionAttribute = (CodexPropertyAttribute)Attribute.GetCustomAttribute(interfaceProperty, typeof(CodexPropertyAttribute));

            if (descriptionAttribute != null && descriptionAttribute.Show)
            {
                auxiliaryProperty = Instantiate(propertyPrefab);
                auxiliaryProperty.transform.SetParent(propertyArea, false);
                auxiliaryProperty.transform.FindChild("Label").GetComponent<Text>().text = interfaceProperty.Name;

                propertyObjectList.Add(auxiliaryProperty);
            }
            else
                continue;
        }
    }

    public void Awake()
    {
        propertyObjectList = new List<GameObject>();
        interfaceProperties = new List<PropertyInfo>();
    }

    public void Update()
    {
        if (interfaceProperties != null)
        {
            foreach (GameObject propertyObject in propertyObjectList)
            {
                try
                {
                    Transform labelObject = propertyObject.transform.FindChild("Label");
                    Transform valueObject = propertyObject.transform.FindChild("Value");

                    string value = interfaceProperties.Find(x => x.Name == labelObject.GetComponent<Text>().text).GetValue(HUD.Instance.terminal.selectedActor, null).ToString();
                    valueObject.GetComponent<Text>().text = value;
                }
                catch(Exception e)
                {
                    break;
                }
            }
        }
    }
}
