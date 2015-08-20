using UnityEngine;
using System.Collections;

public enum MainCameraOffsetDirection
{
    Reset,
    Left,
    Right,
    Front,
    Back
}

public class MainCamera : Singleton<MainCamera>
{
    public Transform target;
    public bool follow = true;

    public Vector3 relativePosition;

    public float offsetChangeTime = 0.2f;
    public float horizontalOffset;

    private Vector3 offset;

    private void FollowTarget()
    {
        transform.position = target.position + relativePosition + offset;
    }

    public void SetOffset(MainCameraOffsetDirection offset)
    {
        iTween.StopByName(GetInstanceID() + "Offset");

        float targetHorizontalOffset = 0;

        switch (offset)
        {
            case MainCameraOffsetDirection.Left:
                targetHorizontalOffset = -horizontalOffset;
                break;
            case MainCameraOffsetDirection.Right:
                targetHorizontalOffset = horizontalOffset;
                break;
            case MainCameraOffsetDirection.Front:
                break;
            case MainCameraOffsetDirection.Back:
                break;
            default:
                break;
        }

        Vector3 targetOffset = new Vector3(targetHorizontalOffset, 0, 0);

        iTween.ValueTo(gameObject, iTween.Hash("name", GetInstanceID() + "Offset", "from", this.offset, "to", targetOffset, "time", offsetChangeTime, "onupdate", "UpdateOffset"));
    }

    private void UpdateOffset(Vector3 offset)
    {
        this.offset = offset;
    }

    void LateUpdate()
    {
        if (follow)
            FollowTarget();
    }

    void Start()
    {
        offset = Vector3.zero;
    }
}
