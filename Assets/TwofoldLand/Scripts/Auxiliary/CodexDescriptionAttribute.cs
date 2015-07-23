using System;

[AttributeUsage(AttributeTargets.All)]
public class CodexDescriptionAttribute : System.Attribute
{
    public readonly string Description;

    public readonly int StaminaCost;

    public CodexDescriptionAttribute(string description, int staminaCost)
    {
        this.Description = description;
        this.StaminaCost = staminaCost;
    }
}
