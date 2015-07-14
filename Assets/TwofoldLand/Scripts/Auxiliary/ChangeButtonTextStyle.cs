using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class ChangeButtonTextStyle : MonoBehaviour
{
    #region Properties
    public Text text;
    private FontStyle originalFontStyle;
    #endregion

    #region Methods
    public void ToggleBold(bool toggle)
    {
        if (toggle)
            text.fontStyle = FontStyle.Bold;
        else
            text.fontStyle = originalFontStyle;
    }

    public void ToggleItalic(bool toggle)
    {
        if (toggle)
            text.fontStyle = FontStyle.Italic;
        else
            text.fontStyle = originalFontStyle;
    }

    public void ToggleBoldAndItalic(bool toggle)
    {
        if (toggle)
            text.fontStyle = FontStyle.BoldAndItalic;
        else
            text.fontStyle = originalFontStyle;
    }
    #endregion

    #region MonoBehaviour
    void Start()
    {
        originalFontStyle = text.fontStyle;
    }

    void Update()
    {

    }
    #endregion
}
