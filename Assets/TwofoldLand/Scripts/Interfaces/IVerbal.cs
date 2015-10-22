using UnityEngine;
using System.Collections;

public interface IVerbal : IBase
{
    [CodexProperty("Verbal messages which are expressed by this Entity", false, true)]
    string[] Messages { get; }

    [CodexMethod(null, 0, false)]
    void OnMessageEnd();
}
