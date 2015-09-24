using UnityEngine;
using System.Collections;

public class HoveringItem : MonoBehaviour
{
    [Header("Visual Effects")]
    public float rotateDegreesBySecond = 120;
    [Space(10)]
    public float lightFadeTime;
    public iTween.EaseType lightFadeEaseType;
    [Space(10)]
    public Vector3 absorptionForce;
    public float absorptionRotationMultiplier;

    private Light light;
    private Material material;
    private Rigidbody rb;

    public void JumpAndFade()
    {
        Color fadedColor = material.color;
        fadedColor.a = 0;

        GetComponent<Collider>().enabled = false;
        rb.AddForce(absorptionForce);
        rotateDegreesBySecond *= absorptionRotationMultiplier;

        iTween.ValueTo(gameObject, iTween.Hash("from", light.intensity, "to", 0, "time", lightFadeTime, "easetype", lightFadeEaseType, "onupdate", "UpdateLightIntensity", "oncomplete", "Destroy"));
        iTween.ValueTo(gameObject, iTween.Hash("from", material.color, "to", fadedColor, "time", lightFadeTime, "easetype", lightFadeEaseType, "onupdate", "UpdateMaterialFade"));
    }

    public void Rotate()
    {
        transform.Rotate(new Vector3(0, 1, 0) * rotateDegreesBySecond * Time.deltaTime);
    }

    private void UpdateLightIntensity(float value)
    {
        light.intensity = value;
    }

    private void UpdateMaterialFade(Color value)
    {
        material.color = value;
    }

    void OnDestroy()
    {
        iTween.Stop(gameObject);
    }

    void Awake()
    {
        light = GetComponentInChildren<Light>();
        material = GetComponent<MeshRenderer>().material;
        rb = GetComponent<Rigidbody>();
    }
}
