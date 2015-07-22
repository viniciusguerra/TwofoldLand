using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class AuraContainer : MonoBehaviour
{
	#region Properties
    [SerializeField]
    private int amount;

    public float rotateDegreesBySecond = 18;
    
    public int Amount { get { return amount; } }

    private Rigidbody rigidbody;
	#endregion

	#region Methods
    public void Destroy()
    {
        Destroy(this.gameObject);
    }

    private void Idle()
    {
        transform.Rotate(new Vector3(0, 1, 0) * rotateDegreesBySecond * Time.deltaTime);
    }
	#endregion

	#region MonoBehaviour
	void Start()
	{
        rigidbody = GetComponent<Rigidbody>();
	}

	void Update()
	{
        Idle();
	}
	#endregion
}
