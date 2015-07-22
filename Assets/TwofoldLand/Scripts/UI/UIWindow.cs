using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIWindow : MonoBehaviour
{
    #region Properties
    public bool startsActive;
    public float uiFadeTime = 0.15f;
    public iTween.EaseType uiFadeEaseType = iTween.EaseType.easeOutCubic;

    protected bool isVisible;

    public bool IsVisible { get { return isVisible; } }

    private CanvasGroup canvasGroup;
    #endregion

    #region Methods
    public virtual void Show()
    {
        SetVisibility(true);
    }

    public virtual void Hide()
    {
        SetVisibility(false);
    }

    public virtual void Toggle()
    {
        SetVisibility(!isVisible);
    }

    public void SetVisibility(bool visible)
    {
        isVisible = visible;

        string tweenName = GetInstanceID() + "Toggle";
        iTween.StopByName(tweenName);

        canvasGroup.interactable = isVisible;
        canvasGroup.blocksRaycasts = isVisible;

        iTween.ValueTo(gameObject, iTween.Hash("from", canvasGroup.alpha, "to", isVisible ? 1 : 0, "time", uiFadeTime, "easetype", uiFadeEaseType, "onupdate", "UpdateAlpha", "name", tweenName));
    }
    #endregion

    private void UpdateAlpha(float a)
    {
        canvasGroup.alpha = a;
    }

    #region MonoBehaviour
    public virtual void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.interactable = startsActive;
        canvasGroup.blocksRaycasts = startsActive;
        canvasGroup.alpha = startsActive ? 1 : 0;
    }
    #endregion
}
