using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class Path : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool setPosition;

    private IEnumerator SetTargetMarkerPosition()
    {
        while (setPosition)
        {
            SetTargetMarkerPosition(((CustomStandaloneInputModule)EventSystem.current.currentInputModule).GetLastPointerEventDataPublic(-1));
            yield return null;
        }

        HideTargetMarker();
    }

    private void ShowTargetMarker(PointerEventData data)
    {
        if (data.pointerCurrentRaycast.gameObject != null && !data.pointerCurrentRaycast.gameObject.tag.Equals(GlobalDefinitions.PathTag))
            return;

        Player.Instance.MovingEntity.MoveToPosition(data.pointerCurrentRaycast.worldPosition);
        TargetMarker.Instance.Set(data.pointerCurrentRaycast.worldPosition);

        setPosition = true;

        StartCoroutine(SetTargetMarkerPosition());
    }

    private void SetTargetMarkerPosition(PointerEventData data)
    {
        if (!data.pointerCurrentRaycast.gameObject.tag.Equals(GlobalDefinitions.PathTag))
            return;

        Player.Instance.MovingEntity.MoveToPosition(data.pointerCurrentRaycast.worldPosition);
        TargetMarker.Instance.SetPosition(data.pointerCurrentRaycast.worldPosition);
    }

    private void HideTargetMarker()
    {
        TargetMarker.Instance.HideTween();
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerId == -1)
        {
            ShowTargetMarker(eventData);
        }
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerId == -1)
        {
            setPosition = false;
        }
    }
}
