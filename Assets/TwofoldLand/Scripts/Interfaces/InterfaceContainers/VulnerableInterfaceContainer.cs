using UnityEngine;
using System.Collections;

public class VulnerableInterfaceContainer : InterfaceContainer
{
    public override System.Type InterfaceType
    {
        get
        {
            return typeof(IVulnerable);
        }
    }
}
