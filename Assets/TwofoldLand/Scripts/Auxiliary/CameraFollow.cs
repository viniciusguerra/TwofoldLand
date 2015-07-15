using UnityEngine;
using System.Collections;

public class CameraFollow : SingletonMonoBehaviour<CameraFollow>
{
    public Transform target;
    public bool follow = true;
    public Vector3 relativePosition;

    private void FollowTarget()
    {
        transform.position = target.position + relativePosition;
    }

    void LateUpdate()
    {
        if (follow)
            FollowTarget();
    } 
}
