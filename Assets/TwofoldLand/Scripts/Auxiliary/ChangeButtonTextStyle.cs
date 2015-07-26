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
    public void ToggleBold()
    {
        text.fontStyle = text.fontStyle == FontStyle.Bold ? originalFontStyle : FontStyle.Bold;
    }

    public void ToggleItalic()
    {
        text.fontStyle = text.fontStyle == FontStyle.Italic ? originalFontStyle : FontStyle.Italic;
    }

    public void ToggleBoldAndItalic()
    {
        text.fontStyle = text.fontStyle == FontStyle.BoldAndItalic ? originalFontStyle : FontStyle.BoldAndItalic;
    }

    public void SetNormal()
    {
        text.fontStyle = FontStyle.Normal;
    }

    public void SetBold()
    {
        text.fontStyle = FontStyle.Bold;
    }
    #endregion

    #region MonoBehaviour
    void Awake()
    {
        originalFontStyle = text.fontStyle;
    }
    #endregion
}
