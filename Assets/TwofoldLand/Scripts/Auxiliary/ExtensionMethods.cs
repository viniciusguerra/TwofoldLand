using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

static public class ExtensionMethods
{
    #region Methods
    /// <summary>
    /// Gets or add a component. Usage example:
    /// BoxCollider boxCollider = transform.GetOrAddComponent<BoxCollider>();
    /// </summary>
    static public T GetOrAddComponent<T>(this Component child) where T : Component
    {
        T result = child.GetComponent<T>();
        if (result == null)
        {
            result = child.gameObject.AddComponent<T>();
        }
        return result;
    }

    static public void SetLayerRecursively(this GameObject go, int newLayer)
    {
        go.layer = newLayer;

        foreach (Transform child in go.transform)
        {
            child.gameObject.layer = newLayer;

            if (child.childCount > 0)
            {
                SetLayerRecursively(child.gameObject, newLayer);
            }
        }
    }
    #endregion
}
