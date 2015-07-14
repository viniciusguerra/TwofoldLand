using System;

[AttributeUsage(AttributeTargets.All)]
public class CodexDescriptionAttribute : System.Attribute
{
    public readonly string Description;

    public CodexDescriptionAttribute(string description)
    {
        this.Description = description;
    }
}
