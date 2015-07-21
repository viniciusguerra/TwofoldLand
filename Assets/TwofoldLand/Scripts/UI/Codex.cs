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

    public Canvas canvas;
    public float propertyHeight;
    public float methodHeight;
    #endregion

    #region Methods
    public override void Toggle()
    {
        base.Toggle();

        if (isVisible)
        {
            PopulateInterfaceList();
        }
    }

    public override void Show()
    {
        base.Show();

        PopulateInterfaceList();
    }

    public override void Hide()
    {
        base.Hide();

        ClearShownInterface();
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
        Show();

        currentInterfaceType = Type.GetType(name);

        interfaceNameText.text = currentInterfaceType.Name;

        interfaceLevelText.text = String.Format("Lvl.{0}", Ricci.Skills.Find(x => x.interfaceContainer.InterfaceType == currentInterfaceType).level);

        DisplayProperties();

        DisplayMethods();
    }

    public void DisplayInterface(bool toggle, string name)
    {
        //will be called whenever a toggle changes value
        //if the value is changed to false, the Interface shouldn't be shown
        if (toggle == false)
            return;

        Show();

        currentInterfaceType = Type.GetType(name);

        interfaceNameText.text = currentInterfaceType.Name;

        interfaceLevelText.text = String.Format("Lvl.{0}", Ricci.Skills.Find(x => x.interfaceContainer.InterfaceType == currentInterfaceType).level);

        DisplayProperties();

        DisplayMethods();
    }

    private void ClearShownInterface()
    {
        interfaceNameText.text = "Select an Interface";

        interfaceLevelText.text = string.Empty;

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
    }

    private RectTransform CreatePropertyUI(PropertyInfo propertyInfo)
    {
        GameObject property = Instantiate<GameObject>(propertyPrefab);
        property.transform.SetParent(propertyListPanel, false);

        RectTransform rt = property.GetComponent<RectTransform>();
        Rect prefabRect = propertyPrefab.GetComponent<RectTransform>().rect;
        rt.rect.Set(prefabRect.x, prefabRect.y, prefabRect.width, prefabRect.height);

        List<Transform> childList = new List<Transform>(property.transform.GetComponentsInChildren<Transform>());

        childList.Find(x => x.name == "Access").GetComponent<Text>().text = propertyInfo.GetGetMethod().IsPublic ? "public" : "private";
        childList.Find(x => x.name == "Return").GetComponent<Text>().text = propertyInfo.PropertyType.Name;
        childList.Find(x => x.name == "Name").GetComponent<Text>().text = propertyInfo.Name;

        CodexDescriptionAttribute descriptionAttribute = (CodexDescriptionAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(CodexDescriptionAttribute));
        childList.Find(x => x.name == "Description").GetComponent<Text>().text = descriptionAttribute != null ? descriptionAttribute.Description : string.Empty;        

        return rt;
    }

    private RectTransform CreateMethodUI(MethodInfo methodInfo)
    {
        GameObject method = Instantiate<GameObject>(methodPrefab);
        method.transform.SetParent(methodListPanel, false);

        RectTransform rt = method.GetComponent<RectTransform>();
        Rect prefabRect = methodPrefab.GetComponent<RectTransform>().rect;
        rt.rect.Set(prefabRect.x, prefabRect.y, prefabRect.width, prefabRect.height);

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

        return rt;
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
        propertyHeight = propertyPrefab.GetComponent<RectTransform>().rect.height;
        methodHeight = methodPrefab.GetComponent<RectTransform>().rect.height;

        methodList = new List<RectTransform>();
        propertyList = new List<RectTransform>();
    }
    #endregion
}
