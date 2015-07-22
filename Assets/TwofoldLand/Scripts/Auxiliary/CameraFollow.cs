using UnityEngine;
using System.Collections;

public class CameraFollow : Singleton<CameraFollow>
{
    public Transform target;
    public bool follow = true;
    public Vector3 relativePosition;
    public float offsetChangeTime = 0.2f;

    private Vector3 offset;

    private void FollowTarget()
    {
        transform.position = target.position + relativePosition + offset;
    }

    public void AddOffset(Vector3 offset)
    {
        iTween.StopByName(GetInstanceID() + "Offset");
        iTween.ValueTo(gameObject, iTween.Hash("name", GetInstanceID() + "Offset", "from", this.offset, "to", offset, "time", offsetChangeTime, "onupdate", "UpdateOffset"));
    }

    public void Center()
    {
        AddOffset(Vector3.zero);
    }

    private void UpdateOffset(Vector3 offset)
    {
        this.offset = offset;
    }

    void LateUpdate()
    {
        offset = Vector3.zero;

        if (follow)
            FollowTarget();
    } 
}
