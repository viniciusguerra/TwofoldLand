using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Skill
{
    [SerializeField]
    private int level;

    public int Level
    {
        get { return level; }
    }

    [SerializeField]
    private SkillData skillData;

    public bool CanLevelUp()
    {
        return (level < skillData.maxLevel);
    }

    public void LevelUp()
    {
        ++level;
    }

    public int GetAuraToNextLevel()
    {
        return skillData.auraToNextLevel[level - 1];
    }

    public Type GetInterfaceType()
    {
        return skillData.InterfaceType;
    }    

    public Skill(SkillData skillData)
    {
        this.skillData = skillData;
        level = 1;
    }
}
