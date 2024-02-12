using System;

[AttributeUsage(AttributeTargets.Method)]
public class ConsoleCommand : Attribute
{
    public string alias { get; private set; }
    public string help { get; private set; }

    public ConsoleCommand(string alias = null, string help = null)
    {
        this.alias = alias;
        this.help = help;
    }
}
