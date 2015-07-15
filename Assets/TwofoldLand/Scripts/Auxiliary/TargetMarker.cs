using UnityEngine;
using System.Collections;

public class TargetMarker : SingletonMonoBehaviour<TargetMarker>
{
    public float yOffset;
    public Vector3 scaleGrowth;
    public float blinkTime = 0.3f;
    public float timeToHide = 0.6f;
    private Material material;

    public Color auxiliaryColor;

    public void Set(Vector3 target)
    {
        iTween.StopByName(gameObject, name + "Scale");
        iTween.StopByName(gameObject, name + "Color");

        transform.localScale = Vector3.one;        
        
        Blink();
        SetPosition(target);
    }

    public void SetPosition(Vector3 target)
    {
        ShowInstantly();

        transform.position = target + new Vector3(0, yOffset, 0);
    }

    public void HideTween()
    {
        auxiliaryColor = material.color;
        auxiliaryColor.a = 0;

        if (iTween.tweens.Exists(x => x.ContainsValue(name + "Color")))
            ResumeHiding();
        else
            StartHiding();
    }

    public void HideInstantly()
    {
        iTween.StopByName(gameObject, name + "Scale");
        iTween.StopByName(gameObject, name + "Color");

        transform.localScale = Vector3.one;

        auxiliaryColor = material.color;
        auxiliaryColor.a = 0;

        material.color = auxiliaryColor;
    }

    private void ShowInstantly()
    {
        auxiliaryColor = material.color;
        auxiliaryColor.a = 1;
        material.color = auxiliaryColor;
    }    

    private void Blink()
    {
        iTween.PunchScale(gameObject, iTween.Hash("name", name + "Scale", "amount", scaleGrowth, "time", blinkTime));
    }

    private void StartHiding()
    {
        iTween.ColorTo(gameObject, iTween.Hash("name", name + "Color", "color", auxiliaryColor, "delay", blinkTime, "time", timeToHide));
    }

    private void ResumeHiding()
    {
        iTween.Resume(gameObject, name + "Color");
    }

    void Start()
    {
        material = GetComponent<MeshRenderer>().material;

        HideInstantly();
    }

    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals(GlobalDefinitions.PlayerTag))            
            HideInstantly();
    }
}
