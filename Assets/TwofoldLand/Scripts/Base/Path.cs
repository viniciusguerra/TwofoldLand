using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Path : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    public void OnPointerClick(PointerEventData data)
    {
        if (data.pointerId == -1)
        {
            Ricci.Instance.MoveToPosition(data.pointerCurrentRaycast.worldPosition);
            TargetMarker.Instance.SetPosition(data.pointerCurrentRaycast.worldPosition);
        }
    }

    public void OnDrag(PointerEventData data)
    {
        if (data.pointerId == -1)
        {
            Ricci.Instance.MoveToPosition(data.pointerCurrentRaycast.worldPosition);
            TargetMarker.Instance.SetPosition(data.pointerCurrentRaycast.worldPosition);
        }
    }
}
