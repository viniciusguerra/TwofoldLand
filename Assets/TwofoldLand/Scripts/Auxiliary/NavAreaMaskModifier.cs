using UnityEngine;
using System.Collections;

public class NavAreaMaskModifier : MonoBehaviour
{
    public NavMeshAgent target;
    public string[] layersToAdd;
    public string[] layersToRemove;

    [ContextMenu("Update Target Area Mask")]
    public void UpdateTargetAreaMask()
    {
        foreach (string layerName in layersToAdd)
        {
            target.areaMask = target.areaMask | (1 << NavMesh.GetAreaFromName(layerName));
        }

        foreach (string layerName in layersToRemove)
        {
            target.areaMask = target.areaMask & ~(1 << NavMesh.GetAreaFromName(layerName));
        }
    }
}
