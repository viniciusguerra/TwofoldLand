using UnityEngine;
using System.Collections;

public class MovingEntity : Entity
{
    [Header("Movement")]
    public float lookSpeed = 45;

    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private Animator animator;

    public void LookAt(Vector3 target)
    {
        string tweenName = GetInstanceID() + "LookTo";

        iTween.StopByName(tweenName);
        iTween.LookTo(gameObject, iTween.Hash("name", tweenName, "axis", "y", "looktarget", target, "speed", lookSpeed));
    }

    public void MoveToPosition(Vector3 targetPosition)
    {
        iTween.Stop(gameObject);
        agent.SetDestination(targetPosition);
        agent.Resume();
    }

    public void StopMoving()
    {
        agent.Stop();
    }

    public virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();        
    }

    public virtual void Start()
    {
        animator.GetBehaviour<AgentVelocityGetter>().navMeshAgent = agent;
    }
}
