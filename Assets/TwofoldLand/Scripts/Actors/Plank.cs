using UnityEngine;
using System.Collections;
using System;

public class Plank : Actor, IKinetic
{
    [SerializeField]
    private bool isLoose;

    public bool IsLoose
    {
        get { return isLoose; }
    }

    [SerializeField]
    private int looseThreshold;

    public int LooseThreshold
    {
        get { return looseThreshold; }
    }

    [SerializeField]
    private int force;
    [SerializeField]
    private float maxDragDistance;
    [SerializeField]
    private float distanceToFloor;
    [SerializeField]
    private float zOffsetToRicci;    
    private Transform originalParent;

    private static float dragMagnitude = 0.5f;

    private Rigidbody rb;
    private NavMeshObstacle obstacle;

    void IKinetic.Drag()
    {
        if (isLoose)
        {
            if(Vector3.Distance(transform.position, Ricci.Instance.transform.position) > maxDragDistance)
                HUD.Instance.log.Push("Object too far to be dragged");
            else
            {
                RaycastHit hitInfo;

                Physics.Raycast(new Ray(transform.position, Vector3.down), out hitInfo);

                float dragHeight = hitInfo.transform.position.y + distanceToFloor;        

                iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(transform.position.x, dragHeight, transform.position.z), "time", 0.5f, "oncomplete", "StartDrag"));
            }
        }
        else
            HUD.Instance.log.Push("Object cannot be dragged because it's not loose");
    }

    

    void IKinetic.Pull()
    {
        Loosen();

        if(isLoose)
            rb.AddForce((Ricci.Instance.transform.position - transform.position).normalized * force * GetKinematicLevel());
        else
            HUD.Instance.log.Push("Force not enough for loosing object");
    }

    void IKinetic.Push()
    {
        Loosen();

        if (isLoose)
            rb.AddForce((transform.position - Ricci.Instance.transform.position).normalized * force * GetKinematicLevel());
        else
            HUD.Instance.log.Push("Force not enough for loosing object");
    }

    void IKinetic.Push(object direction)
    {
        Vector3 directionVector = Vector3.zero;

        string directionString = direction.ToString();

        if (directionString == "left")
            directionVector = Vector3.left;

        if (directionString == "right")
            directionVector = Vector3.right;

        if (directionString == "forward")
            directionVector = Vector3.forward;

        if (directionString == "back")
            directionVector = Vector3.back;

        if (directionVector == Vector3.zero)
        {
            HUD.Instance.log.Push("Invalid direction");
            return;
        }

        Loosen();

        if (isLoose)
        {
            rb.AddForce((directionVector).normalized * force * GetKinematicLevel());
        }
        else
            HUD.Instance.log.Push("Force not enough for loosing object");
    }

    private void Loosen()
    {
        if (GetKinematicLevel() * force > looseThreshold)
        {
            isLoose = true;
            rb.isKinematic = false;
        }
    }

    private int GetKinematicLevel()
    {
        return Ricci.Instance.skillList.Find(x => x.GetInterfaceType().Name.Equals("IKinetic")).Level;
    }

    private void StartDrag()
    {
        StartCoroutine(DragCoroutine());
    }

    IEnumerator DragCoroutine()
    {
        rb.isKinematic = true;

        //Cursor.visible = false;

        transform.SetParent(Ricci.Instance.transform, true);

        obstacle.enabled = false;

        while (HUD.Instance.terminal.selectedActor == this)
        {
            //rb.MovePosition(transform.position + (new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y"))).normalized * dragMagnitude);

            //if (Vector3.Distance(transform.position, Ricci.Instance.transform.position) >= GetKinematicLevel() * maxDragDistance)
            //    break;

            yield return null;
        }

        transform.SetParent(originalParent, true);

        obstacle.enabled = true;

        rb.isKinematic = false;

        //Cursor.visible = true;
    }

    public override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody>();
        obstacle = GetComponent<NavMeshObstacle>();
        originalParent = transform.parent;
    }
}
