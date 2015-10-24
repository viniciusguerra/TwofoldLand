﻿using UnityEngine;
using System.Collections;

public interface IKinetic : IBase
{
    [CodexProperty("If true, the object can be dragged, else, it cannot", true, true)]
    bool IsLoose
    { get; }

    [CodexMethod("Enables dragging of an Actor if it is loose", 15, true)]
    void Drag();

    [CodexMethod("Pushes the Actor away from you", 30, true)]
    void Push();

    [CodexMethod("Pushes the Actor in the given direction: left, right, forward or back", 30, true)]
    void Push(object direction);

    [CodexMethod("Pulls the Actor towards you", 30, true)]
    void Pull();
}
