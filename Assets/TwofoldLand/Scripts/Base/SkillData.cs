using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SkillData : CollectableData, ISerializationCallbackReceiver
{
    #region Properties
    public string interfaceName;

    private Type interfaceType;

    public Type InterfaceType
    {
        get
        {
            SetInterfaceType();
            return interfaceType;
        }
    }

    public int maxLevel;

    public List<int> auraToNextLevel;
    #endregion

    #region Methods
    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        maxLevel = Mathf.Max(1, maxLevel);

        if(string.IsNullOrEmpty(interfaceName))
        {
            interfaceName = string.Empty;
        }

        SetInterfaceType();
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {

    }

    private void SetInterfaceType()
    {
        Type type = Type.GetType(interfaceName);

        if (type == null)
        {
            interfaceName = "Interface Type Null";
            interfaceType = null;
        }
        else
            interfaceType = type;
    }
    #endregion
}