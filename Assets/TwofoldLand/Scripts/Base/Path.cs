using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

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
        if (!data.pointerCurrentRaycast.gameObject.tag.Equals(GlobalDefinitions.PathTag))
            return;

        Ricci.Instance.MoveToPosition(data.pointerCurrentRaycast.worldPosition);
        TargetMarker.Instance.Set(data.pointerCurrentRaycast.worldPosition);

        setPosition = true;

        StartCoroutine(SetTargetMarkerPosition());
    }

    private void SetTargetMarkerPosition(PointerEventData data)
    {
        if (!data.pointerCurrentRaycast.gameObject.tag.Equals(GlobalDefinitions.PathTag))
            return;

        Ricci.Instance.MoveToPosition(data.pointerCurrentRaycast.worldPosition);
        TargetMarker.Instance.SetPosition(data.pointerCurrentRaycast.worldPosition);
    }

    private void HideTargetMarker()
    {
        TargetMarker.Instance.HideTween();
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (data.pointerId == -1)
        {
            ShowTargetMarker(data);
        }
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (data.pointerId == -1)
        {
            setPosition = false;
        }
    }
}
