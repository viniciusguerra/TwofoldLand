using UnityEngine;
using System.Collections;

public interface IUnlockable : IBase
{
    [CodexDescription("The binary number which, converted to decimal, unlocks the object")]
    string BinaryKey
    {
        get;
    }

    [CodexDescription("True if the object is unlocked, false otherwise")]
    bool Unlocked
    {
        get;
    }

    [CodexDescription("Brings up the Binary Conversion UI")]
    void Unlock();

    [CodexDescription("Unlocks when giving the adequate key")]
    void Unlock(object key);

    [CodexDescription("Opens and closes if unlocked")]
    void Toggle();
}
