using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class Codex : UIWindow
{
    #region Properties
    public InterfaceToggleList interfaceList;

    public Text interfaceNameText;
    public Text interfaceLevelText;
    public RectTransform propertyPanel;
    public RectTransform propertyListPanel;
    public RectTransform methodPanel;
    public RectTransform methodListPanel;

    public GameObject propertyPrefab;
    public GameObject methodPrefab;

    public List<RectTransform> propertyList;
    public List<RectTransform> methodList;

    public Vector2 panelPadding;

    private Type currentInterfaceType;

    private float propertyHeight;
    private float methodHeight;
    private float propertyPanelHeight;
    private float methodPanelHeight;
    #endregion

    #region Methods
    public override void Toggle(bool show)
    {
        base.Toggle(show);

        if (show)
        {
            PopulateInterfaceList();
        }
    }

    private void PopulateInterfaceList()
    {
        string[] interfaceNameArray = new string[Ricci.Skills.Count];

        for (int i = 0; i < Ricci.Skills.Count; i++)
        {
            interfaceNameArray[i] = Ricci.Skills[i].interfaceContainer.InterfaceType.Name;
        }

        interfaceList.DisplayInterfaces(interfaceNameArray);
    }

    public void DisplayInterface(string name)
    {
        gameObject.SetActive(true);

        ClearDisplay();

        PopulateInterfaceList();

        currentInterfaceType = Type.GetType(name);

        interfaceNameText.text = currentInterfaceType.Name;

        interfaceLevelText.text = String.Format("Lvl.{0}", Ricci.Skills.Find(x => x.interfaceContainer.InterfaceType == currentInterfaceType).level);

        DisplayProperties();

        DisplayMethods();
    }

    public void DisplayInterface(bool toggle, string name)
    {
        if (toggle)
        {
            gameObject.SetActive(true);

            ClearDisplay();

            PopulateInterfaceList();

            currentInterfaceType = Type.GetType(name);

            interfaceNameText.text = currentInterfaceType.Name;

            interfaceLevelText.text = String.Format("Lvl.{0}", Ricci.Skills.Find(x => x.interfaceContainer.InterfaceType == currentInterfaceType).level);

            DisplayProperties();

            DisplayMethods();
        }
    }

    private void ClearDisplay()
    {
        interfaceNameText.text = "Interface";

        interfaceLevelText.text = "Lvl.";

        ClearRectTransformList(propertyList);

        ClearRectTransformList(methodList);        
    }

    private void DisplayProperties()
    {
        PropertyInfo[] properties = currentInterfaceType.GetProperties();

        foreach (PropertyInfo propertyInfo in properties)
        {
            propertyList.Add(CreatePropertyUI(propertyInfo));
        }

        UpdatePanelHeight(propertyListPanel, propertyPanelHeight, propertyHeight, propertyList.Count);

        OrderRectTransformList(propertyList.ToArray(), propertyHeight);
    }

    private void DisplayMethods()
    {
        MethodInfo[] methods = currentInterfaceType.GetMethods();

        foreach (MethodInfo methodInfo in methods)
        {
            //avoid getting Property Methods when displaying Methods
            if(!methodInfo.Name.StartsWith("get_") && !methodInfo.Name.StartsWith("set_"))
                methodList.Add(CreateMethodUI(methodInfo));
        }

        UpdatePanelHeight(methodListPanel, methodPanelHeight, methodHeight, methodList.Count);

        OrderRectTransformList(methodList.ToArray(), methodHeight);
    }

    private RectTransform CreatePropertyUI(PropertyInfo propertyInfo)
    {
        GameObject property = Instantiate<GameObject>(propertyPrefab);

        RectTransform rt = property.GetComponent<RectTransform>();

        List<Transform> childList = new List<Transform>(property.transform.GetComponentsInChildren<Transform>());

        childList.Find(x => x.name == "Access").GetComponent<Text>().text = propertyInfo.GetGetMethod().IsPublic ? "public" : "private";
        childList.Find(x => x.name == "Return").GetComponent<Text>().text = propertyInfo.PropertyType.Name;
        childList.Find(x => x.name == "Name").GetComponent<Text>().text = propertyInfo.Name;

        CodexDescriptionAttribute descriptionAttribute = (CodexDescriptionAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(CodexDescriptionAttribute));
        childList.Find(x => x.name == "Description").GetComponent<Text>().text = descriptionAttribute != null ? descriptionAttribute.Description : string.Empty;

        property.transform.SetParent(propertyListPanel);

        return rt;
    }

    private RectTransform CreateMethodUI(MethodInfo methodInfo)
    {
        GameObject method = Instantiate<GameObject>(methodPrefab);

        RectTransform rt = method.GetComponent<RectTransform>();

        List<Transform> childList = new List<Transform>(method.transform.GetComponentsInChildren<Transform>());

        childList.Find(x => x.name == "Access").GetComponent<Text>().text = methodInfo.IsPublic ? "public" : "private";
        childList.Find(x => x.name == "Return").GetComponent<Text>().text = methodInfo.ReturnType.Name;
        childList.Find(x => x.name == "Name").GetComponent<Text>().text = methodInfo.Name;

        CodexDescriptionAttribute descriptionAttribute = (CodexDescriptionAttribute)Attribute.GetCustomAttribute(methodInfo, typeof(CodexDescriptionAttribute));
        childList.Find(x => x.name == "Description").GetComponent<Text>().text = descriptionAttribute != null ? descriptionAttribute.Description : string.Empty;

        string parametersString = "(";
        ParameterInfo[] parameters = methodInfo.GetParameters();

        if (parameters.Length > 0)
        {
            for (int i = 0; i < parameters.Length - 1; i++)
            {
                parametersString += parametersString.GetType().Name + ", ";
            }

            parametersString += parameters[parameters.Length - 1].Name;
        }

        parametersString += ")";

        childList.Find(x => x.name == "Parameters").GetComponent<Text>().text = parametersString;

        method.transform.SetParent(methodListPanel);

        return rt;
    }

    protected void UpdatePanelHeight(RectTransform panel, float panelHeight, float elementHeight, int elementCount)
    {
        Rect panelRect = panel.rect;

        panel.sizeDelta = new Vector2(0, Mathf.Max(panelHeight, elementHeight * elementCount));
    }

    private void OrderRectTransformList(RectTransform[] array, float elementHeight)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i].anchoredPosition = new Vector2(panelPadding.x, -elementHeight * i);
        }
    }

    private void ClearRectTransformList(List<RectTransform> list)
    {
        foreach (RectTransform rt in list)
            Destroy(rt.gameObject);

        list.Clear();
    }
    #endregion

    #region MonoBehaviour
    void Awake()
    {
        propertyPanelHeight = propertyPanel.rect.height;
        methodPanelHeight = methodPanel.rect.height;
        propertyHeight = propertyPrefab.GetComponent<RectTransform>().rect.height;
        methodHeight = methodPrefab.GetComponent<RectTransform>().rect.height;

        methodList = new List<RectTransform>();
        propertyList = new List<RectTransform>();
    }
    #endregion
}
