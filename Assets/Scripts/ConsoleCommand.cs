using System;

[AttributeUsage(AttributeTargets.Method)]
public class ConsoleCommand : Attribute
{
    public string alias { get; private set; } = null;

    public ConsoleCommand(string alias = null)
    {
        this.alias = alias;
    }
}
