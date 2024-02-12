using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using TMPro;
using System;

public class Command
{
    public string name;
    public MethodInfo methodInfo;
    public object commandObject;
    public string help;

    public Command(string name, MethodInfo methodInfo, object commandObject, string help)
    {
        this.name = name;
        this.methodInfo = methodInfo;
        this.commandObject = commandObject;
        this.help = help;
    }
}

public class SimpleConsole : MonoBehaviour
{
    [SerializeField] Color errorColor;
    [SerializeField] Color warningColor;
    [SerializeField] Color logColor;
    [SerializeField] KeyCode consoleKey = KeyCode.BackQuote;
    [SerializeField] GameObject consoleLogTextPrefab;

    GameObject consolePanel;
    List<Command> consoleMethods = new List<Command>();
    List<string> history = new List<string>() { "" };
    TMP_InputField commandInputField;
    Transform contentTransform;
    Transform scrollRectTransform;

    string tempCommand = "";
    int historyIndex = 0;

    bool historyActive = false;

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void Start()
    {
        consolePanel = transform.Find("Console Panel").gameObject;
        commandInputField = consolePanel.transform.Find("Input Field").GetComponent<TMP_InputField>();
        scrollRectTransform = consolePanel.transform.Find("Log Window");
        contentTransform = scrollRectTransform.Find("Mask").Find("Content");

        if (consolePanel.activeSelf)
        {
            commandInputField.ActivateInputField();
        }

        FindAllConsoleCommands();
    }

    private void Update()
    {
        HandleConsoleActivation();

        if (consolePanel.activeSelf)
        {
            ListenCommands();
            HandleHistory();
        }
    }

    private void FindAllConsoleCommands()
    {
        MonoBehaviour[] objects = FindObjectsOfType<MonoBehaviour>();

        for (int i = 0; i < objects.Length; i++)
        {
            MethodInfo[] mInfos = objects[i].GetType().GetMethods();
            for (int j = 0; j < mInfos.Length; j++)
            {
                object[] attributes = mInfos[j].GetCustomAttributes(typeof(ConsoleCommand), false);
                if (attributes.Length > 0)
                {
                    ConsoleCommand command = (ConsoleCommand)attributes[0];
                    string methodName = command.alias ?? mInfos[j].Name;
                    consoleMethods.Add(new Command(methodName, mInfos[j], objects[i], command.help));
                }
            }
        }
    }

    private void HandleConsoleActivation()
    {
        if (Input.GetKeyDown(consoleKey))
        {
            consolePanel.SetActive(!consolePanel.activeSelf);

            if (consolePanel.activeSelf)
            {
                ClearInputField();
                commandInputField.ActivateInputField();
            }
        }
    }

    private void ListenCommands()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            string[] tokenizedCommand = commandInputField.text.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
            if (tokenizedCommand.Length == 0)
            {
                ClearInputField();
                return;
            }

            int commandIndex = 0;
            bool isCommandExists = false;
            for (int i = 0; i < consoleMethods.Count; i++)
            {
                isCommandExists = consoleMethods[i].name == tokenizedCommand[0] || isCommandExists;

                if (consoleMethods[i].name == tokenizedCommand[0])
                {
                    commandIndex = i;
                }
            }
            if (!isCommandExists)
            {
                ClearInputField();
                return;
            }

            Command command = consoleMethods[commandIndex];
            ParameterInfo[] parameters = command.methodInfo.GetParameters();
            if (parameters.Length != tokenizedCommand.Length - 1)
            {
                ClearInputField();
                return;
            }

            if (parameters.Length == 0)
            {
                command.methodInfo.Invoke(command.commandObject, null);
            }
            else
            {
                object[] args = new object[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    args[i] = Convert.ChangeType(tokenizedCommand[i + 1], parameters[i].ParameterType);
                }

                command.methodInfo.Invoke(command.commandObject, args);
            }

            history.Add(commandInputField.text);
            ClearInputField();
        }
    }

    private void HandleHistory()
    {
        if (history.Count > 0)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (!historyActive)
                {
                    tempCommand = commandInputField.text;
                }
                historyIndex = Math.Min(historyIndex + 1, history.Count - 1);
                commandInputField.text = history[historyIndex];
                commandInputField.stringPosition = commandInputField.text.Length;
                historyActive = true;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                historyIndex = Math.Max(historyIndex - 1, 0);
                commandInputField.text = history[historyIndex];
                commandInputField.stringPosition = commandInputField.text.Length;

                if (historyIndex == 0 && historyActive)
                {
                    historyActive = false;
                    commandInputField.text = tempCommand;
                    commandInputField.stringPosition = commandInputField.text.Length;
                }
            }
        }
    }

    private void ClearInputField()
    {
        commandInputField.text = "";
        commandInputField.ActivateInputField();
        historyIndex = 0;
        tempCommand = "";
        historyActive = false;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        GameObject consoleLogText = Instantiate(consoleLogTextPrefab, contentTransform);
        TMP_Text textObject = consoleLogText.GetComponent<TMP_Text>();

        if (type == LogType.Error || type == LogType.Exception || type == LogType.Assert)
        {
            textObject.color = errorColor;
            textObject.text = "> " + logString;
            textObject.text += "\n" + stackTrace;
        }
        else if (type == LogType.Warning)
        {
            textObject.color = warningColor;
            textObject.text = "> " + logString;
        }
        else
        {
            textObject.color = logColor;
            textObject.text = "> " + logString;
        }

        scrollRectTransform.GetComponent<ContentScroller>().AdjustContent();
    }

    [ConsoleCommand("available-commands", "Lists all available commands")]
    public void AvailableCommands()
    {
        string output = "";

        for (int i = 0; i < consoleMethods.Count; i++)
        {
            output += consoleMethods[i].name;
            ParameterInfo[] parameters = consoleMethods[i].methodInfo.GetParameters();
            if (parameters.Length > 0)
            {
                output += " -";
                for (int j = 0; j < parameters.Length; j++)
                {
                    output += " " + parameters[j].ParameterType.Name;
                }
            }

            if (consoleMethods[i].help != null)
            {
                output += " - " + consoleMethods[i].help;
            }
            
            output += "\n";
        }

        Debug.Log(output);
    }
}
