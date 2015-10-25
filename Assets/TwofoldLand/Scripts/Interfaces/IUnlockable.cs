using UnityEngine;
using System.Collections;

public interface IUnlockable : IBase
{
    [CodexProperty(false, true)]
    string BinaryKey
    { get; }

    [CodexProperty(true, true)]
    bool Unlocked
    { get; }

    [CodexMethod(40, true)]
    void Unlock();

    [CodexMethod(0, true)]
    void Unlock(object key);

    [CodexMethod(15, true)]
    void Toggle();
}
