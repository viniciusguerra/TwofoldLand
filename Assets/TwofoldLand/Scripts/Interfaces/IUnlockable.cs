using UnityEngine;
using System.Collections;

public interface IUnlockable : IBase
{
    [CodexDescription("The binary number which, converted to decimal, unlocks the object", 0)]
    string BinaryKey
    {
        get;
    }

    [CodexDescription("True if the object is unlocked, false otherwise", 0)]
    bool Unlocked
    {
        get;
    }

    [CodexDescription("Brings up the Binary Conversion UI", 0)]
    void Unlock();

    [CodexDescription("Unlocks when giving the adequate key", 8)]
    void Unlock(object key);

    [CodexDescription("Opens and closes if unlocked", 2)]
    void Toggle();
}
