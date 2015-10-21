using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class Log : MonoBehaviour
{
    #region Properties
    public int messageLimit = 5;
    public float messageTime = 2.6f;
    public float clearTime = 1;

    public GameObject logMessagePrefab;

    private List<Text> messageList;
    #endregion

    #region Methods
    public void ShowMessage(string message)
    {
        if (messageList.Count >= messageLimit)
        {
            iTween.StopByName(messageList[0].gameObject.GetInstanceID().ToString());
            Destroy(messageList[0].gameObject);
            messageList.RemoveAt(0);            
        }

        messageList.Add(CreateMessage(message));
    }

    private Text CreateMessage(string messageText)
    {
        GameObject messageGameObject = Instantiate(logMessagePrefab);
        messageGameObject.transform.SetParent(transform, false);

        Text messageTextComponent = messageGameObject.GetComponent<Text>();
        messageTextComponent.text = messageText;

        iTween.ValueTo(messageGameObject, iTween.Hash( "name",messageGameObject.GetInstanceID().ToString(),
                                                       "onupdatetarget", messageGameObject,
                                                       "onupdate", "ClearAlpha",
                                                       "from", messageTextComponent.color.a,
                                                       "to", 0,
                                                       "delay", messageTime,
                                                       "time", clearTime,
                                                       "oncompletetarget", gameObject,
                                                       "oncomplete", "ClearMessage",
                                                       "oncompleteparams", messageTextComponent));

        return messageTextComponent;
    }

    private void ClearMessage(Text message)
    {
        messageList.Remove(message);

        Destroy(message.gameObject);
    }
    #endregion

    #region MonoBehaviour
    void Start()
    {
        messageList = new List<Text>(messageLimit);
    }

    void Update()
    {

    }
    #endregion
}
