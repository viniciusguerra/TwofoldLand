using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class Aura : MonoBehaviour
{
	#region Properties
    public int auraAmount;

    [Header("Visual Effects")]
    public float rotateDegreesBySecond = 120;
    [Space(10)]
    public float lightFadeTime;
    public iTween.EaseType lightFadeEaseType;
    [Space(10)]
    public Vector3 absorptionForce;
    public float absorptionRotationMultiplier;

    private new Rigidbody rigidbody;
    private Light light;
    private Material material;
	#endregion

	#region Methods
    public void Absorb()
    {
        Color fadedColor = material.color;
        fadedColor.a = 0;

        rigidbody.AddForce(absorptionForce);
        rotateDegreesBySecond *= absorptionRotationMultiplier;

        iTween.ValueTo(gameObject, iTween.Hash("from", light.intensity, "to", 0, "time", lightFadeTime, "easetype", lightFadeEaseType, "onupdate", "UpdateLightIntensity", "oncomplete", "Destroy"));
        iTween.ValueTo(gameObject, iTween.Hash("from", material.color, "to", fadedColor, "time", lightFadeTime, "easetype", lightFadeEaseType, "onupdate", "UpdateMaterialFade"));      
    }

    private void UpdateLightIntensity(float value)
    {
        light.intensity = value;
    }

    private void UpdateMaterialFade(Color value)
    {
        material.color = value;
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private void Idle()
    {
        transform.Rotate(new Vector3(0, 1, 0) * rotateDegreesBySecond * Time.deltaTime);
    }    
	#endregion

	#region MonoBehaviour
    void OnDestroy()
    {
        iTween.Stop(gameObject);
    }

	void Start()
	{
        rigidbody = GetComponent<Rigidbody>();
        light = GetComponentInChildren<Light>();
        material = GetComponent<MeshRenderer>().material;
	}

	void Update()
	{
        Idle();
	}
	#endregion
}
