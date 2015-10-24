using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class BinaryConversion : UIWindow
{
	#region Properties
    public float closeDelay = 2;
    public float fillTime = 0.333f;

    public List<Button> digits;
    public List<Button> currentDigits;
    public string currentBinaryKey;
    public Entity currentTarget;

    public Text currentDecimalText;
    public Text targetDecimalText;
    public Image successFill;

    public Color neutralDigitColor;
    public Color successDigitColor;
	#endregion

	#region Methods
    public void Show(Entity unlockableEntity)
    {
        base.Show();

        successFill.fillAmount = 0;

        currentTarget = unlockableEntity;

        int digitCount = (currentTarget as IUnlockable).BinaryKey.Length;

        currentDigits = digits.GetRange(0, digitCount);

        InitializeCurrentDigits();

        targetDecimalText.text = Convert.ToInt32((currentTarget as IUnlockable).BinaryKey, 2).ToString();
    }

    public override void Hide()
    {
        base.Hide();

        if (currentTarget != null)
        {
            currentTarget.OnCommandFailure("Binary conversion cancelled");
            currentTarget = null;
        }

        foreach (Button digit in currentDigits)
        {
            digit.gameObject.SetActive(false);
        }
    }

    public void Validate()
    {
        currentBinaryKey = string.Empty;

        foreach (Button digit in currentDigits)
        {
            currentBinaryKey += digit.GetComponentInChildren<Text>().text;
        }

        SetCurrentDecimalText();

        if (currentBinaryKey == (currentTarget as IUnlockable).BinaryKey)
        {
            foreach (Button digit in currentDigits)
            {
                digit.interactable = false;
            }

            iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1, "time", fillTime, "onupdate", "UpdateSuccessFill", "oncomplete", "CloseSuccess"));
        }
    }

    private void SetDigitValue(Button digit, int value)
    {
        digit.GetComponentInChildren<Text>().text = value.ToString();
    }

    private void SetCurrentDecimalText()
    {
        currentDecimalText.text = Convert.ToInt32(currentBinaryKey, 2).ToString();
    }

    private void SetCurrentDigitsColor(Color color)
    {
        foreach (Button digit in currentDigits)
        {
            digit.GetComponentInChildren<Text>().color = color;
        }
    }

    private void InitializeCurrentDigits()
    {
        foreach (Button digit in currentDigits)
        {
            digit.gameObject.SetActive(true);
            digit.GetComponentInChildren<Text>().color = neutralDigitColor;
            digit.GetComponentInChildren<Text>().text = "0";
            digit.interactable = true;
        }

        currentBinaryKey = "0";

        SetCurrentDecimalText();
    }

    public void ToggleDigit(Text digitText)
    {
        digitText.text = digitText.text == "0" ? "1" : "0";
    }    

    private void CloseSuccess()
    {
        SetCurrentDigitsColor(successDigitColor);

        StartCoroutine(WaitToCloseCoroutine());   
    }

    public IEnumerator WaitToCloseCoroutine()
    {
        yield return new WaitForSeconds(closeDelay);

        (currentTarget as IUnlockable).Unlock(currentBinaryKey);

        currentTarget = null;

        Hide();
    }    

    public void UpdateSuccessFill(float value)
    {
        successFill.fillAmount = value;
    }
	#endregion

	#region MonoBehaviour
	public override void Start()
	{
        base.Start();

        currentDigits = new List<Button>();
	}

	void Update()
	{
        if(IsVisible)
        {
            if (!HUD.Instance.terminal.HasSelectedActor())
                Hide();
        }
	}
	#endregion
}
