using UnityEngine;

public class ConsoleTest : MonoBehaviour
{
    [ConsoleCommand]
    public void TestConsoleCommand()
    {
        Debug.Log("testing");
    }

    [ConsoleCommand]
    public void TestConsoleCommandWithArgs(string argument, int number, float floatNumber, bool boolVar)
    {
        if (boolVar)
        {
            Debug.Log("bool");
        }
        Debug.Log(floatNumber);
        Debug.Log(number);
        Debug.Log("testing " + argument);
    }

    [ConsoleCommand("testCommand")]
    public void TestConsoleCommandWithAlias()
    {
        Debug.Log("testing");
    }

    [ConsoleCommand]
    public void TestConsoleWarning()
    {
        Debug.LogWarning("Warning log command");
    }

    [ConsoleCommand]
    public void TestConsoleError()
    {
        Debug.LogError("Error log command");
    }
}
