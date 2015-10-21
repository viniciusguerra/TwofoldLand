using System;

[AttributeUsage(AttributeTargets.All)]
public class CodexMethodAttribute : System.Attribute
{
    public readonly string Description;

    public readonly int StaminaCost;

    public CodexMethodAttribute(string description, int staminaCost)
    {
        this.Description = description;
        this.StaminaCost = staminaCost;
    }
}

[AttributeUsage(AttributeTargets.All)]
public class CodexPropertyAttribute : System.Attribute
{
    public readonly string Description;

    public readonly bool Show;

    public CodexPropertyAttribute(string description, bool showOnInfoPanel)
    {
        this.Description = description;
        this.Show = showOnInfoPanel;
    }
}
