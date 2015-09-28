using UnityEngine;
using System.Collections;

public interface IKinetic : IBase
{
    [CodexDescription("If true, the object can be manipulated, else, it cannot", 0)]
    bool IsLoose
    { get; }

    [CodexDescription("How much force needs to be applied to this object so that it will break loose", 0)]
    int LooseThreshold
    { get; }

    [CodexDescription("Enables dragging of an Actor if it is loose", 15)]
    void Drag();

    [CodexDescription("Pushes the Actor away from you", 5)]
    void Push();

    [CodexDescription("Pulls the Actor towards you", 5)]
    void Pull();
}
