using UnityEngine;
using System.Collections;

public class TargetMarker : SingletonMonoBehaviour<TargetMarker>
{    
    public float yOffset;
    public float hideDistance = 1;

    private MeshRenderer meshRenderer;

    public void SetPosition(Vector3 target)
    {
        StopAllCoroutines();

        Show();
        transform.position = target + new Vector3(0, yOffset, 0);
        StartCoroutine(HideOnArrival());
    }

    private void Show()
    {
        meshRenderer.enabled = true;
    }

    public void Hide()
    {
        meshRenderer.enabled = false;
        StopAllCoroutines();
    }

    private IEnumerator HideOnArrival()
    {
        while (Vector3.Distance(transform.position,Ricci.Instance.transform.position) > hideDistance)
            yield return null;

        Hide();
    }    

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        HideOnArrival();
    }
}
