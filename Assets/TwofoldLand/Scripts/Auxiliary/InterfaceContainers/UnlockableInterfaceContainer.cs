using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class UnlockableInterfaceContainer : Skill
{
    public override System.Type InterfaceType
    {
        get
        {
            return typeof(IUnlockable);
        }
    }
}
