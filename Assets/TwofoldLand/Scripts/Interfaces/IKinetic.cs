using UnityEngine;
using System.Collections;

public interface IKinetic : IBase
{
    [CodexProperty("If true, the object can be manipulated, else, it cannot", true)]
    bool IsLoose
    { get; }

    [CodexMethod("Enables dragging of an Actor if it is loose", 15)]
    void Drag();

    [CodexMethod("Pushes the Actor away from you", 5)]
    void Push();

    [CodexMethod("Pushes the Actor in the given direction: left, right, forward or back", 5)]
    void Push(object direction);

    [CodexMethod("Pulls the Actor towards you", 5)]
    void Pull();
}
