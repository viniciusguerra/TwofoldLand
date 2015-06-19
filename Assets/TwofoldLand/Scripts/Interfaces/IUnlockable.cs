using UnityEngine;
using System.Collections;

public interface IUnlockable : IAbstraction
{
    void Unlock();
    void Unlock(object decimalKey);
    void Toggle();
}
