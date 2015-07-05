using UnityEngine;
using System.Collections;

public class CameraFollow : SingletonMonoBehaviour<CameraFollow>
{
    [Header("Initial Transform")]
    public Vector3 relativePosition;
    public Vector3 cameraRelativeRotation;

    [Header("Configuration")]
    public GameObject cameraGameObject;    
    public Collider boundsCollider;
    public GameObject target;

    [Space(10)]
    public bool follow;
    public Vector3 motionSpeed;

    [Header("Runtime")]
    public Vector3 closestPoint;    
    public Vector3 motionDirection;    

    private IEnumerator FollowTarget()
    {
        while(follow)
        {
            gameObject.transform.Translate(new Vector3(motionDirection.x * motionSpeed.x,
                                                       motionDirection.y * motionSpeed.y,
                                                       motionDirection.z * motionSpeed.z) * Time.deltaTime);

            yield return null;
        }
    }

    public void SetFollow(bool follow)
    {
        this.follow = follow;

        if(follow)
            StartCoroutine(FollowTarget());
    }

    public void WatchTarget()
    {
        if (!boundsCollider.bounds.Contains(target.transform.position))
        {
            closestPoint = boundsCollider.bounds.ClosestPoint(target.transform.position);
            motionDirection = (target.transform.position - closestPoint).normalized;
        }
        else
        {
            closestPoint = Vector3.zero;
            motionDirection = Vector3.zero;
        }
    }

    public void InitializeTransform()
    {
        transform.position = relativePosition;
        cameraGameObject.transform.rotation = Quaternion.Euler(cameraRelativeRotation);

        transform.position += target.transform.position;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        if(!boundsCollider.bounds.Contains(target.transform.position))
            Gizmos.DrawLine(closestPoint, target.transform.position);
    }

    void Start()
    {
        if (target != null)
            InitializeTransform();

        SetFollow(true);           
    }

    void LateUpdate()
    {
        WatchTarget();
    }    
}
