using UnityEngine;
using System.Collections;
using System;

public class Obstacle : Entity, IKinetic
{
    [SerializeField]
    private bool isLoose;

    public bool IsLoose
    {
        get { return isLoose; }
    }

    [SerializeField]
    private bool eliminateObstacleOnLoosen;
    [SerializeField]
    private int levelToLoosen;
    [SerializeField]
    private int force;
    [SerializeField]
    private float maxDragDistance;
    [SerializeField]
    private float distanceToFloor;
    [SerializeField]
    private float distanceToPlayer;    
    private Transform originalParent;

    private static float dragMagnitude = 0.5f;

    private Rigidbody rb;
    private NavMeshObstacle obstacle;

    void IKinetic.Drag()
    {
        if (isLoose)
        {
            if (Vector3.Distance(transform.position, Player.Instance.transform.position) > maxDragDistance)
            {
                OnCommandFailure("Object too far to be dragged");
            }
            else
            {
                float dragHeight = Player.Instance.transform.position.y + distanceToFloor;

                iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(transform.position.x, dragHeight, transform.position.z), "time", 0.5f, "oncomplete", "StartDrag"));
            }
        }
        else
        {
            OnCommandFailure("Object is not loose, can't be dragged");
        }
    }

    

    void IKinetic.Pull()
    {
        if (GetKinematicLevel() >= levelToLoosen)
            Loosen();

        if (isLoose)
        {
            rb.AddForce((Player.Instance.transform.position - transform.position).normalized * force * GetKinematicLevel());
            OnCommandSuccess();
        }
        else
        {
            OnCommandFailure("Not strong enough to pull object");
        }
    }

    void IKinetic.Push()
    {
        if (GetKinematicLevel() >= levelToLoosen)
            Loosen();

        if (isLoose)
        {
            rb.AddForce((transform.position - Player.Instance.transform.position).normalized * force * GetKinematicLevel());
            OnCommandSuccess();
        }
        else
        {
            OnCommandFailure("Not strong enough to push object");
        }
    }

    void IKinetic.Push(object direction)
    {
        Vector3 directionVector = Vector3.zero;

        string directionString = direction.ToString();

        if (directionString == "left")
            directionVector = transform.TransformDirection(Vector3.left);

        if (directionString == "right")
            directionVector = transform.TransformDirection(Vector3.right);

        if (directionString == "forward")
            directionVector = transform.TransformDirection(Vector3.forward);

        if (directionString == "back")
            directionVector = transform.TransformDirection(Vector3.back);

        if (directionString == "up")
            directionVector = transform.TransformDirection(Vector3.up);

        if (directionString == "down")
            directionVector = transform.TransformDirection(Vector3.down);

        if (directionVector == Vector3.zero)
        {
            HUD.Instance.log.ShowMessage("Invalid direction");
            return;
        }

        if (GetKinematicLevel() >= levelToLoosen)        
            Loosen();

        if (isLoose)
        {
            rb.AddForce((directionVector).normalized * force * GetKinematicLevel());
            OnCommandSuccess();
        }
        else
        {
            OnCommandFailure("Not strong enough to push object");
        }
    }

    public void Loosen()
    {        
        isLoose = true;
        rb.isKinematic = false;
        obstacle.enabled = eliminateObstacleOnLoosen ? false : true;        
    }

    private int GetKinematicLevel()
    {
        return Player.Instance.skillList.Find(x => x.GetInterfaceType().Name.Equals("IKinetic")).Level;
    }

    private void StartDrag()
    {
        StartCoroutine(DragCoroutine());
    }

    IEnumerator DragCoroutine()
    {
        rb.isKinematic = true;

        //Cursor.visible = false;

        transform.SetParent(Player.Instance.transform, true);

        //obstacle.enabled = false;

        OnCommandSuccess();

        while (HUD.Instance.terminal.selectedActor != null && HUD.Instance.terminal.selectedActor.Entity == this)
        {
            //rb.MovePosition(transform.position + (new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y"))).normalized * dragMagnitude);

            //if (Vector3.Distance(transform.position, Ricci.Instance.transform.position) >= GetKinematicLevel() * maxDragDistance)
            //    break;

            yield return null;
        }

        transform.SetParent(originalParent, true);

        //obstacle.enabled = true;

        rb.isKinematic = false;        

        //Cursor.visible = true;
    }

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        obstacle = GetComponent<NavMeshObstacle>();
        originalParent = transform.parent;
    }
}
