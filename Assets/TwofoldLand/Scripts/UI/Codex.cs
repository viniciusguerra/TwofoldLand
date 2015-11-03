using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using SmartLocalization;

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

    [Space(20)]
    public Text interfaceNameText;

    [Space(20)]
    public UIWindow interfaceLevelPanel;
    public Text interfaceLevelText;
    public Text levelUpCostText;

    [Space(10)]
    public Button levelUpButton;
    public Color levelUpEnabledColor;
    public Color levelUpDisabledColor;

    [Space(20)]    
    public RectTransform propertyPanel;
    public Scrollbar propertyPanelScrollbar;
    public RectTransform propertyListPanel;
    public RectTransform methodPanel;
    public Scrollbar methodPanelScrollbar;
    public RectTransform methodListPanel;    

    [Space(20)]
    public GameObject propertyPrefab;
    public GameObject methodPrefab;

    [Space(20)]
    public List<RectTransform> propertyList;
    public List<RectTransform> methodList;
    public List<RectTransform> spellList;

    private Type currentInterfaceType;    
    #endregion

    #region Methods
    public override void Toggle()
    {
        if(!isVisible)
        {
            HUD.Instance.storage.Hide();
        }

        base.Toggle();

        if (isVisible)
        {
            MainCamera.Instance.SetOffset(MainCameraOffsetDirection.Right);

            ClearShownInterface();

            DisplayInterfaceArea();

            interfaceList.DisplayInterfaces(Player.Instance.GetAllSkillInterfaces());

            UpdateInterfaceLevelArea();
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

        interfaceList.DisplayInterfaces(Player.Instance.GetAllSkillInterfaces());

        UpdateInterfaceLevelArea();
    }

    public override void Hide()
    {
        base.Hide();

        MainCamera.Instance.SetOffset(MainCameraOffsetDirection.Reset);

        ClearShownInterface();
    }

    public void DisplayInterfaceArea()
    {
        spellArea.Hide();
        spellAreaButton.SetNormal();
        interfaceArea.Show();
        interfaceAreaButton.SetBold();
    }

    public void UpdateInterfaceLevelArea()
    {
        if (currentInterfaceType != null)
        {
            Skill currentSkill = Player.Instance.GetSkill(currentInterfaceType);

            interfaceLevelText.text = String.Format("Lvl.{0}", Player.Instance.skillList.Find(x => x.GetInterfaceType() == currentInterfaceType).Level);

            if (currentSkill.CanLevelUp())
            {
                interfaceLevelPanel.Show();

                int levelUpCost = currentSkill.GetCostToLevelUp();
                levelUpCostText.text = levelUpCost.ToString();

                if (Player.Instance.Aura >= levelUpCost)
                {
                    levelUpButton.GetComponentInChildren<Text>().color = levelUpEnabledColor;
                    levelUpButton.enabled = true;
                }
                else
                {
                    levelUpButton.GetComponentInChildren<Text>().color = levelUpDisabledColor;
                    levelUpButton.enabled = false;
                }
            }
            else
                interfaceLevelPanel.Hide();
        }
        else
            interfaceLevelPanel.Hide();
    }

    public void LevelCurrentSkillUp()
    {
        Player.Instance.LevelSkillUp(currentInterfaceType);

        UpdateInterfaceLevelArea();
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

    public void DisplayInterface(string name)
    {
        Show();

        DisplayInterfaceArea();

        currentInterfaceType = Type.GetType(name);

        interfaceNameText.text = currentInterfaceType.Name;        

        DisplayProperties();

        DisplayMethods();

        UpdateInterfaceLevelArea();        
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

        interfaceLevelText.text = String.Format("Lvl.{0}", Player.Instance.skillList.Find(x => x.GetInterfaceType() == currentInterfaceType).Level);

        DisplayProperties();

        DisplayMethods();

        UpdateInterfaceLevelArea();
    }

    private void ClearShownInterface()
    {
        currentInterfaceType = null;

        interfaceNameText.text = LanguageManager.Instance.GetTextValue("UI.Codex.SelectSkillMessage");

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

        propertyPanelScrollbar.value = 1;
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

        methodPanelScrollbar.value = 1;
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
        CodexPropertyAttribute descriptionAttribute = (CodexPropertyAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(CodexPropertyAttribute));

        //Don't create UI if there's no Attribute attached to the Property
        if (descriptionAttribute == null || descriptionAttribute.ShowOnCodex == false)
            return null;

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
        childList.Find(x => x.name == "Description").GetComponent<Text>().text = descriptionAttribute != null ? LanguageManager.Instance.GetTextValue(propertyInfo.DeclaringType.FullName + "." + propertyInfo.Name) : string.Empty;        

        return rt;
    }

    private RectTransform CreateMethodUI(MethodInfo methodInfo)
    {
        CodexMethodAttribute descriptionAttribute = (CodexMethodAttribute)Attribute.GetCustomAttribute(methodInfo, typeof(CodexMethodAttribute));

        //Don't create UI if there's no Attribute attached to the Method
        if (descriptionAttribute == null || descriptionAttribute.ShowOnCodex == false)
            return null;

        GameObject method = Instantiate<GameObject>(methodPrefab);
        method.transform.SetParent(methodListPanel, false);

        RectTransform rt = method.GetComponent<RectTransform>();
        Rect prefabRect = methodPrefab.GetComponent<RectTransform>().rect;
        rt.rect.Set(prefabRect.x, prefabRect.y, prefabRect.width, prefabRect.height);

        List<Transform> childList = new List<Transform>(method.transform.GetComponentsInChildren<Transform>());

        //Info comes from Interface, Interfaces have no access definition
        //childList.Find(x => x.name == "Access").GetComponent<Text>().text = methodInfo.IsPublic ? "public" : "private";
        //childList.Find(x => x.name == "Return").GetComponent<Text>().text = methodInfo.ReturnType.Name;
        childList.Find(x => x.name == "Name").GetComponent<Text>().text = methodInfo.Name;
        childList.Find(x => x.name == "Description").GetComponent<Text>().text = descriptionAttribute != null ? LanguageManager.Instance.GetTextValue(methodInfo.DeclaringType.FullName + "." + methodInfo.Name + (methodInfo.GetParameters().Length == 0 ? "" : methodInfo.GetParameters().Length.ToString())) : string.Empty;

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
        {
            if(rt != null && rt.gameObject != null)
                Destroy(rt.gameObject);
        }

        list.Clear();
    }
    #endregion

    #region MonoBehaviour
    void Awake()
    {
        methodList = new List<RectTransform>();
        propertyList = new List<RectTransform>();
        spellList = new List<RectTransform>();

        UpdateSpells();
    }
    #endregion
}
