using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Text))]
public class FeedbackUI : SingletonMonoBehaviour<FeedbackUI>
{
    #region Properties
    public int messageLimit = 5;
    public float messageTime = 2.6f;
    public float messageHeight = 20;
    public float clearTime = 1;

    private Text textStyle;
    private List<Text> messageList;
    #endregion

    #region Methods
    public void Log(string message)
    {
        if (messageList.Count >= messageLimit)
        {
            iTween.StopByName(messageList[0].gameObject.GetInstanceID().ToString());
            Destroy(messageList[0].gameObject);
            messageList.RemoveAt(0);            
        }

        messageList.Add(CreateMessage(message));

        UpdateMessagePositions();
    }

    private Text CreateMessage(string text)
    {
        GameObject feedbackMessage = new GameObject("FeedbackMessage");
        feedbackMessage.transform.parent = transform;
        feedbackMessage.AddComponent<Text>();
        feedbackMessage.AddComponent<TextAlphaClearer>();

        Text messageText = feedbackMessage.GetComponent<Text>();

        messageText.rectTransform.sizeDelta = new Vector2(0, messageHeight);
        messageText.alignment = textStyle.alignment;
        messageText.font = textStyle.font;
        messageText.fontSize = textStyle.fontSize;
        messageText.fontStyle = textStyle.fontStyle;
        messageText.color = textStyle.color;
        messageText.horizontalOverflow = textStyle.horizontalOverflow;
        messageText.verticalOverflow = textStyle.verticalOverflow;

        messageText.rectTransform.pivot = new Vector2(0, 0.5f);
        messageText.rectTransform.anchorMax = new Vector2(0, 0.5f);
        messageText.rectTransform.anchorMin = new Vector2(0, 0.5f);

        messageText.text = text;

        iTween.ValueTo(gameObject, iTween.Hash("name",feedbackMessage.GetInstanceID().ToString(),
                                               "onupdatetarget", feedbackMessage,
                                               "onupdate", "ClearAlpha",
                                               "from", textStyle.color.a,
                                               "to", 0,
                                               "delay", messageTime,
                                               "time", clearTime,
                                               "oncomplete", "ClearMessage",
                                               "oncompleteparams", messageText));

        return messageText;
    }

    private void UpdateMessagePositions()
    {
        Text[] messageArray = messageList.ToArray();

        for (int i = 0; i < messageList.Count; i++)
        {
            messageArray[i].rectTransform.anchoredPosition = new Vector2(0, -messageHeight * i);
        }
    }

    private void ClearMessage(Text message)
    {
        messageList.Remove(message);

        Destroy(message.gameObject);

        UpdateMessagePositions();
    }
    #endregion

    #region MonoBehaviour
    void Start()
    {
        messageList = new List<Text>(messageLimit);
        textStyle = GetComponent<Text>();

        GetComponent<RectTransform>().sizeDelta = new Vector2(0, messageHeight * messageLimit);
    }

    void Update()
    {

    }
    #endregion
}
