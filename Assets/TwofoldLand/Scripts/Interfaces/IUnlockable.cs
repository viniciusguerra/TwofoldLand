using UnityEngine;
using System.Collections;

public interface IUnlockable : IBase
{
    [CodexDescription("True if the object is unlocked, false otherwise")]
    bool Unlocked
    {
        get;
    }

    [CodexDescription("Brings up the Binary Conversion UI")]
    void Unlock();

    [CodexDescription("Unlocks when giving the adequate key from the Inventory")]
    void Unlock(object key);

    [CodexDescription("Opens and closes if unlocked")]
    void Toggle();
}
