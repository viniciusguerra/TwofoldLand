using UnityEngine;
using System.Collections;

public class KeepComponentEnabled : MonoBehaviour
{
    public MonoBehaviour component;

    void Update()
    {
        component.enabled = true;
    }
}
