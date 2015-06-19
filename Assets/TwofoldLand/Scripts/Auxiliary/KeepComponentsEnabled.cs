using UnityEngine;
using System.Collections;

public class KeepComponentsEnabled : MonoBehaviour
{
    public MonoBehaviour[] componentsToWatch;

    void Update()
    {
        foreach(MonoBehaviour m in componentsToWatch)
        {
            if (m.enabled == false)
                m.enabled = true;
        }
    }
}
