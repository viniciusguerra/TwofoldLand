using UnityEngine;
using System.Collections;

public interface IUnlockable : IBase
{
    [CodexProperty("The binary number which, converted to decimal, unlocks the object", false, true)]
    string BinaryKey
    { get; }

    [CodexProperty("True if the object is unlocked, false otherwise", true, true)]
    bool Unlocked
    { get; }

    [CodexMethod("Brings up the Binary Conversion UI", 40, true)]
    void Unlock();

    [CodexMethod("Unlocks when giving the correct BinaryKey value or a valid Key from the Storage", 0, true)]
    void Unlock(object key);

    [CodexMethod("Opens and closes if unlocked", 15, true)]
    void Toggle();
}
