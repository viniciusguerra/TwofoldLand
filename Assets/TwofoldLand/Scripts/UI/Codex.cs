using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class Codex : UIWindow
{
	#region Properties
    public float interfaceButtonHeight = 25;

    public GameObject interfaceListPanel;
    public Scrollbar interfaceListScrollbar;

    public List<Button> interfaceButtons;
	#endregion

	#region Methods
    public override void Toggle(bool show)
    {
        base.Toggle(show);

        //if (interfaceButtons.Count != Ricci.Instance.skills.Count)
        //    UpdateInterfaceListPanel();
    }

    private void UpdateInterfaceListPanel()
    {
        interfaceButtons.Clear();

        for (int i = 0; i < Ricci.Instance.skills.Count; i++)
        {
            GameObject buttonGameObject = new GameObject();            
            buttonGameObject.name = Ricci.Instance.skills[i].interfaceContainer.InterfaceType.ToString() + "Button";
            buttonGameObject.transform.SetParent(interfaceListPanel.transform);
            buttonGameObject.AddComponent<Button>();
            buttonGameObject.AddComponent<Image>();

            GameObject bText = new GameObject();
            bText.AddComponent<RectTransform>();
            bText.AddComponent<Text>();
            bText.transform.SetParent(buttonGameObject.transform);
            
            //Button
            Button b = buttonGameObject.GetComponent<Button>();
            b.targetGraphic = buttonGameObject.GetComponent<Image>();           

            //RectTransform
            RectTransform r = buttonGameObject.GetComponent<RectTransform>();
            r.anchorMin = new Vector2(0, 1);
            r.anchorMax = new Vector2(0.87f, 1);
            r.anchoredPosition = new Vector2(0.5f, 1);
            //TODO find width
            r.sizeDelta = new Vector2(10, interfaceButtonHeight);

            //Text
            Text t = buttonGameObject.GetComponentInChildren<Text>();
            t.text = Ricci.Instance.skills[i].interfaceContainer.InterfaceType.ToString();

            //Image
            buttonGameObject.GetComponent<Image>().enabled = false;
        }
    }
	#endregion

	#region MonoBehaviour
	void Start()
	{
        interfaceButtons = new List<Button>();

        UpdateInterfaceListPanel();
	}

	void Update()
	{

	}
	#endregion
}
