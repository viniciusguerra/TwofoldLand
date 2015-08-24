using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class Codex : UIWindow
{
    #region Properties
    [Header("Spell Area")]
    public ChangeButtonTextStyle spellAreaButton;
    public UIWindow spellArea;

    public RectTransform spellListPanel;
    public GameObject spellPrefab;
    public List<Spell> compiledSpells;

    [Header("Interface Area")]
    public ChangeButtonTextStyle interfaceAreaButton;
    public UIWindow interfaceArea;
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
    public List<RectTransform> spellList;

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
            MainCamera.Instance.SetOffset(MainCameraOffsetDirection.Right);

            DisplayInterfaceArea();

            PopulateInterfaceList();
        }
        else
        {
            MainCamera.Instance.SetOffset(MainCameraOffsetDirection.Reset);
        }
    }

    public override void Show()
    {
        base.Show();

        MainCamera.Instance.SetOffset(MainCameraOffsetDirection.Right);

        ClearShownInterface();

        DisplayInterfaceArea();

        PopulateInterfaceList();
    }

    public override void Hide()
    {
        base.Hide();

        MainCamera.Instance.SetOffset(MainCameraOffsetDirection.Reset);

        ClearShownInterface();
    }

    public void DisplayInterfaceArea()
    {
        if (interfaceArea.IsVisible)
            return;

        spellArea.Hide();
        spellAreaButton.SetNormal();
        interfaceArea.Show();
        interfaceAreaButton.SetBold();
    }

    public void DisplaySpellArea()
    {
        if (spellArea.IsVisible)
            return;

        interfaceArea.Hide();
        interfaceAreaButton.SetNormal();
        spellArea.Show();
        spellAreaButton.SetBold();
    }

    [ContextMenu("Update Spells")]
    public void UpdateSpells()
    {
        ClearRectTransformList(spellList);

        DisplaySpells();
    }

    public void AddSpell(Spell spell)
    {
        compiledSpells.Add(spell);

        UpdateSpells();
    }

    public void SetSpellToSlot(RectTransform spellUI)
    {
        if(HUD.Instance.storage.currentSelectedSpellSlot != null)
        {
            Spell selectedSpell = compiledSpells[spellList.IndexOf(spellUI)];

            HUD.Instance.storage.SelectSpell(selectedSpell);
        }
    }

    private void DisplaySpells()
    {
        foreach(Spell s in compiledSpells)
        {
            spellList.Add(CreateSpellUI(s));
        }
    }

    private void PopulateInterfaceList()
    {
        string[] interfaceNameArray = new string[Ricci.Instance.skillList.Count];

        for (int i = 0; i < Ricci.Instance.skillList.Count; i++)
        {
            interfaceNameArray[i] = Ricci.Instance.skillList[i].interfaceContainer.InterfaceType.Name;
        }

        interfaceList.DisplayInterfaces(interfaceNameArray);
    }

    public void DisplayInterface(string name)
    {
        Show();

        DisplayInterfaceArea();

        currentInterfaceType = Type.GetType(name);

        interfaceNameText.text = currentInterfaceType.Name;

        interfaceLevelText.text = String.Format("Lvl.{0}", Ricci.Instance.skillList.Find(x => x.interfaceContainer.InterfaceType == currentInterfaceType).level);

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

        DisplayInterfaceArea();

        currentInterfaceType = Type.GetType(name);

        interfaceNameText.text = currentInterfaceType.Name;

        interfaceLevelText.text = String.Format("Lvl.{0}", Ricci.Instance.skillList.Find(x => x.interfaceContainer.InterfaceType == currentInterfaceType).level);

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

    private RectTransform CreateSpellUI(Spell spell)
    {
        GameObject spellGo = Instantiate<GameObject>(spellPrefab);
        spellGo.transform.SetParent(spellListPanel, false);

        RectTransform rt = spellGo.GetComponent<RectTransform>();
        Rect prefabRect = spellPrefab.GetComponent<RectTransform>().rect;
        rt.rect.Set(prefabRect.x, prefabRect.y, prefabRect.width, prefabRect.height);

        List<Transform> childList = new List<Transform>(spellGo.transform.GetComponentsInChildren<Transform>());

        childList.Find(x => x.name == "SpellTitle").GetComponent<Text>().text = spell.SpellTitle;
        childList.Find(x => x.name == "AuraCost").GetComponent<Text>().text = "Aura Cost: " + "<b>" + spell.AuraCost.ToString() + "</b>";
        childList.Find(x => x.name == "StaminaCost").GetComponent<Text>().text = "Stamina Cost: " + "<b>" + spell.StaminaCost.ToString() + "</b>";

        spellGo.GetComponent<Button>().onClick.AddListener(() => HUD.Instance.codex.SetSpellToSlot(rt));

        return rt;
    }

    private RectTransform CreatePropertyUI(PropertyInfo propertyInfo)
    {
        GameObject property = Instantiate<GameObject>(propertyPrefab);
        property.transform.SetParent(propertyListPanel, false);

        RectTransform rt = property.GetComponent<RectTransform>();
        Rect prefabRect = propertyPrefab.GetComponent<RectTransform>().rect;
        rt.rect.Set(prefabRect.x, prefabRect.y, prefabRect.width, prefabRect.height);

        List<Transform> childList = new List<Transform>(property.transform.GetComponentsInChildren<Transform>());

        //Info comes from Interface, Interfaces have no access definition
        //childList.Find(x => x.name == "Access").GetComponent<Text>().text = propertyInfo.GetGetMethod().IsPublic ? "public" : "private";
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

        //Info comes from Interface, Interfaces have no access definition
        //childList.Find(x => x.name == "Access").GetComponent<Text>().text = methodInfo.IsPublic ? "public" : "private";
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
        spellList = new List<RectTransform>();

        UpdateSpells();
    }
    #endregion
}
