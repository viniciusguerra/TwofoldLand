using UnityEngine;
using System.Collections;

public class CameraTrigger : MonoBehaviour
{
    public Transform targetTransform;    
    public bool disableOnExit = false;

    private bool valid = true;

    void OnTriggerEnter(Collider collider)
    {
        if(valid && collider.CompareTag(GlobalDefinitions.PlayerTag))
            MainCamera.Instance.SetTransform(targetTransform);
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag(GlobalDefinitions.PlayerTag))
        {
            MainCamera.Instance.ResetTransform();

            if(disableOnExit)
                valid = false;
        }
    }
}
