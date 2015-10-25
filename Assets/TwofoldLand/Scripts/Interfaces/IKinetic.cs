using UnityEngine;
using System.Collections;

public interface IKinetic : IBase
{
    [CodexProperty(true, true)]
    bool IsLoose
    { get; }

    [CodexMethod(15, true)]
    void Drag();

    [CodexMethod(30, true)]
    void Push();

    [CodexMethod(30, true)]
    void Push(object direction);

    [CodexMethod(30, true)]
    void Pull();
}
