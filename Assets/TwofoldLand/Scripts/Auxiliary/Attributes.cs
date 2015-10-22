using System;

[AttributeUsage(AttributeTargets.All)]
public class CodexMethodAttribute : System.Attribute
{
    public readonly string Description;
    public readonly int StaminaCost;
    public readonly bool ShowOnCodex;

    public CodexMethodAttribute(string description, int staminaCost, bool showOnCodex)
    {
        this.Description = description;
        this.StaminaCost = staminaCost;
        this.ShowOnCodex = showOnCodex;
    }
}

[AttributeUsage(AttributeTargets.All)]
public class CodexPropertyAttribute : System.Attribute
{
    public readonly string Description;
    public readonly bool ShowOnInfoPanel;
    public readonly bool ShowOnCodex;

    public CodexPropertyAttribute(string description, bool showOnInfoPanel, bool showOnCodex)
    {
        Description = description;
        ShowOnInfoPanel = showOnInfoPanel;
        ShowOnCodex = showOnCodex;
    }
}
