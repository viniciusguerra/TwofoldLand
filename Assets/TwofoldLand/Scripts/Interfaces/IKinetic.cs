using UnityEngine;
using System.Collections;

public interface IKinetic : IBase
{
    [CodexDescription("If true, the object can be manipulated, else, it cannot", 0)]
    bool IsLoose
    { get; }

    [CodexDescription("Enables dragging of an Actor if it is loose", 15)]
    void Drag();

    [CodexDescription("Pushes the Actor away from you", 5)]
    void Push();

    [CodexDescription("Pushes the Actor in the given direction: left, right, forward or back", 5)]
    void Push(object direction);

    [CodexDescription("Pulls the Actor towards you", 5)]
    void Pull();
}
