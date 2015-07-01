using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class FeedbackUI : SingletonMonoBehaviour<FeedbackUI>
{
	#region Properties
    public int messageCount = 4;
    public float messageTime = 1.3f;
    public float messageHeight = 15;
    public float messageWidth = 450;
    public Color messageColor = Color.black;
    public bool bold = false;
    public bool italic = true;

    private bool waitingToClear;
    private List<Text> messageStack;    
	#endregion

	#region Methods
    public void Log(string message)
    {        
        if(messageStack.Count < messageCount)
            messageStack.Add(CreateMessage(message));

        if(messageStack.Count == messageCount)
        {
            Destroy(messageStack[0].gameObject);
            messageStack.RemoveAt(0);
            messageStack.Add(CreateMessage(message));
        }

        UpdateMessagePositions();

        if (!waitingToClear)
            StartCoroutine(WaitToClear());
    }

    private Text CreateMessage(string text)
    {
        GameObject feedbackMessage = new GameObject("FeedbackMessage");
        feedbackMessage.transform.parent = transform;
        feedbackMessage.AddComponent<Text>();        

        Text messageText = feedbackMessage.GetComponent<Text>();

        if(italic)
            messageText.text = "<i>" + text + "</i>";
        if (bold)
            messageText.text = "<b>" + text + "</b>";

        messageText.rectTransform.sizeDelta = new Vector2(messageWidth, messageHeight);
        messageText.alignment = TextAnchor.LowerLeft;
        messageText.font = Resources.Load<Font>(GlobalDefinitions.FeedbackUIFontPath);
        messageText.fontSize = 13;
        messageText.color = messageColor;
        messageText.rectTransform.pivot = new Vector2(0, 0.5f);
        messageText.rectTransform.anchorMax = new Vector2(0, 0.5f);
        messageText.rectTransform.anchorMin = new Vector2(0, 0.5f);

        return messageText;
    }

    private void UpdateMessagePositions()
    {
        Text[] messageArray = messageStack.ToArray();

        for(int i = 0; i < messageStack.Count; i++)
        {
            messageArray[i].rectTransform.anchoredPosition = new Vector2(0, -messageHeight * i);
        }
    }

    private IEnumerator WaitToClear()
    {
        waitingToClear = true;

        yield return new WaitForSeconds(messageTime);

        messageStack.Clear();

        foreach (Transform children in transform)
            Destroy(children.gameObject);

        waitingToClear = false;
    }
	#endregion

	#region MonoBehaviour
	void Start()
	{
        messageStack = new List<Text>(messageCount);

        GetComponent<RectTransform>().sizeDelta = new Vector2(messageWidth, messageHeight * messageCount);
	}

	void Update()
	{

	}
	#endregion
}
