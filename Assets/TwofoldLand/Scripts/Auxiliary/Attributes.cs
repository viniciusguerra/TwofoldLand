﻿using System;

[AttributeUsage(AttributeTargets.All)]
public class CodexMethodAttribute : System.Attribute
{
    public readonly int StaminaCost;
    public readonly bool ShowOnCodex;

    public CodexMethodAttribute(int staminaCost, bool showOnCodex)
    {
        this.StaminaCost = staminaCost;
        this.ShowOnCodex = showOnCodex;
    }
}

[AttributeUsage(AttributeTargets.All)]
public class CodexPropertyAttribute : System.Attribute
{
    public readonly bool ShowOnInfoPanel;
    public readonly bool ShowOnCodex;

    public CodexPropertyAttribute(bool showOnInfoPanel, bool showOnCodex)
    {
        ShowOnInfoPanel = showOnInfoPanel;
        ShowOnCodex = showOnCodex;
    }
}