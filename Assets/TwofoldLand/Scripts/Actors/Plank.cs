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

    private static float dragMagnitude = 0.1f;

    private Rigidbody rb;

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

                iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(transform.position.x, transform.position.y + dragHeight, transform.position.z), "time", 0.5f, "oncomplete", "StartDrag"));
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

        Cursor.visible = false;

        while (HUD.Instance.terminal.selectedActor == this)
        {
            rb.MovePosition(transform.position + (new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y"))).normalized * dragMagnitude);

            if (Vector3.Distance(transform.position, Ricci.Instance.transform.position) >= GetKinematicLevel() * maxDragDistance)
                break;

            yield return null;
        }

        rb.isKinematic = false;

        Cursor.visible = true;
    }

    public override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody>();
    }
}
