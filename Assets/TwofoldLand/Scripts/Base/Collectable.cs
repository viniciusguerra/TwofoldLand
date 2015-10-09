using UnityEngine;
using System.Collections;

public abstract class Collectable : MonoBehaviour
{
    #region Properties
    public virtual bool CollectOnTouch
    {
        get { return true; }
    }

    protected Rigidbody rb;
    #endregion

    #region Methods
    public abstract void Absorb();
    protected abstract void Destroy();
    protected abstract void Idle();
    #endregion

    #region MonoBehaviour()
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        Idle();
    }
    #endregion
}
