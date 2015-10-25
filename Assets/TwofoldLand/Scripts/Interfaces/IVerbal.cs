using UnityEngine;
using System.Collections;

public interface IVerbal : IBase
{
    [CodexProperty(false, true)]
    string[] Messages { get; }

    [CodexMethod(0, false)]
    void OnMessageEnd();
}
