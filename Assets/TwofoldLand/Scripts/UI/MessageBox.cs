using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MessageBox : UIWindow
{
    public Text messageText;

    public Button previousButton;
    public Button nextButton;

    public float messageWritingRate;

    private int currentMessageIndex;
    private IVerbal currentVerbal;

    public IVerbal CurrentVerbal
    {
        get
        {
            return currentVerbal;
        }
    }

    public void Show(IVerbal verbal)
    {
        base.Show();

        currentVerbal = verbal;

        currentMessageIndex = 0;

        previousButton.interactable = false;
        nextButton.interactable = true;

        WriteCurrentMessage();
    }

    public override void Hide()
    {
        base.Hide();

        currentMessageIndex = 0;
        currentVerbal = null;

        previousButton.interactable = false;
        nextButton.interactable = false;

        HUD.Instance.terminal.ClearActorSelection();
    }

    private void WriteCurrentMessage()
    {
        StartCoroutine(MessageWritingCoroutine());
    }

    private IEnumerator MessageWritingCoroutine()
    {
        messageText.text = string.Empty;

        foreach (char character in currentVerbal.Messages[currentMessageIndex])
        {
            messageText.text += character;

            yield return new WaitForSeconds(messageWritingRate);
        }

        if (currentVerbal != null && currentMessageIndex == currentVerbal.Messages.Length - 1)
            currentVerbal.OnMessageEnd();
    }

    public void InterruptMessageWriting()
    {
        StopAllCoroutines();

        messageText.text = currentVerbal.Messages[currentMessageIndex];
    }

    public void Previous()
    {
        currentMessageIndex = Mathf.Max(currentMessageIndex - 1, 0);

        previousButton.interactable = currentMessageIndex == 0 ? false : true;
        nextButton.interactable = true;

        StopAllCoroutines();
        WriteCurrentMessage();
    }

    public void Next()
    {
        currentMessageIndex = Mathf.Min(currentMessageIndex + 1, currentVerbal.Messages.Length - 1);

        previousButton.interactable = true;
        nextButton.interactable = currentMessageIndex == currentVerbal.Messages.Length - 1 ? false : true;

        StopAllCoroutines();
        WriteCurrentMessage();
    }    
}
