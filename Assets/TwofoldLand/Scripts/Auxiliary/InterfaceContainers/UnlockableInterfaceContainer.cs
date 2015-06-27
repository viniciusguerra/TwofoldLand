using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class UnlockableInterfaceContainer : InterfaceContainer
{
    public override System.Type InterfaceType
    {
        get
        {
            return typeof(IUnlockable);
        }
    }
}
