using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SmartLocalization;

public class TextLanguageModifier : MonoBehaviour
{
    public string key;
    private Text textComponent;

    public void UpdateTextAsset(LanguageManager languageManager)
    {
        textComponent.text = languageManager.GetTextValue(key);
    }

    public void Awake()
    {
        textComponent = GetComponent<Text>();
        LanguageManager.Instance.OnChangeLanguage += UpdateTextAsset;
    }
}
