using UnityEngine;
using System.Collections;

public interface IUnlockable : IBase
{
    void Unlock();
    void Unlock(object key);
    void Toggle();
}
