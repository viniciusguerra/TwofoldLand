using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class InterfaceList : MonoBehaviour
{
	#region Properties    
    private GameObject listPanel;

    private float panelHeight;
    private static float buttonHeight = -1;

    private List<Button> interfaceButtonList;
    private static GameObject interfaceButtonPrefab;    
	#endregion

	#region Methods
    public void DisplayInterfaces(string[] interfaceArray)
    {
        ClearInterfaces();

        foreach(string interfaceName in interfaceArray)
        {
            GameObject interfaceButton = GameObject.Instantiate(interfaceButtonPrefab);
            interfaceButtonList.Add(interfaceButton.GetComponent<Button>());

            interfaceButton.GetComponentInChildren<Text>().text = interfaceName;
            interfaceButton.transform.SetParent(listPanel.transform);
        }

        UpdatePanelHeight();

        UpdateButtonPositions();
    }

    private void ClearInterfaces()
    {
        foreach(Button b in interfaceButtonList)
        {
            Destroy(b.gameObject);
        }

        interfaceButtonList.Clear();
    }

    private void UpdateButtonPositions()
    {
        Button[] interfaceButtonArray = interfaceButtonList.ToArray();

        for (int i = 0; i < interfaceButtonList.Count; i++)
        {
            interfaceButtonArray[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -buttonHeight * i);
        }
    }

    private void UpdatePanelHeight()
    {
        RectTransform listPanelRectTransform = listPanel.GetComponent<RectTransform>();

        Rect listPanelRect = listPanelRectTransform.rect;

        listPanelRectTransform.rect.Set(listPanelRect.xMin, listPanelRect.yMin, listPanelRect.width, Mathf.Max(panelHeight, buttonHeight * interfaceButtonList.Count));
    }
	#endregion

	#region MonoBehaviour
	void Awake()
	{
        listPanel = transform.FindChild("InterfaceListPanel").gameObject;

        panelHeight = listPanel.GetComponent<RectTransform>().rect.height;

        if (interfaceButtonPrefab == null)
            interfaceButtonPrefab = Resources.Load<GameObject>("Prefabs/InterfaceButton");

        if(buttonHeight == -1)
            buttonHeight = interfaceButtonPrefab.GetComponent<RectTransform>().sizeDelta.y;

        interfaceButtonList = new List<Button>();
	}

	void Update()
	{
	
	}
	#endregion
}
