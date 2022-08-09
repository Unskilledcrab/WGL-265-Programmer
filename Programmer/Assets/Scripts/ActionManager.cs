using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ActionManager : MonoBehaviour
{
    private Dictionary<string, ActionBase> _actions = new();
    public TMPro.TMP_InputField InputField;

    private void Start()
    {
        foreach (var action in GetComponentsInChildren<ActionBase>())
        {
            foreach (var commandName in action.CommandNames)
            {
                if (!_actions.TryAdd(commandName.ToLower(), action))
                    Debug.LogError($"The command name [{commandName}] is already taken");
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ParseInput();
            InputField.text = string.Empty;
            InputField.ActivateInputField();
        }
    }

    public void ParseInput()
    {
        var command = InputField.text.ToLower().Trim();

        if (command == "stop all" || command == "stop")
        {
            foreach (var action in _actions.Values)
            {
                action.StopAllActions();
            }
            return;
        }

        var tokens = command.Split(' ');
        var actions = _actions.Where(a => command.Contains(a.Key)).Select(a => a.Value).ToHashSet();
        var type = GetCommandType(command);
        var number = "0.0";
        foreach (var token in tokens)
        {
            if (float.TryParse(token, out var fnum) || int.TryParse(token, out var num))
                number = token;
        }

        if (!actions.Any())
        {
            Debug.LogWarning($"[{command}] does not compute");
            return;
        }

        foreach (var action in actions)
        {
            switch (type)
            {
                case CommandType.Manual:
                    action.Action();
                    break;
                case CommandType.Iterative:
                    action.StartIterativeAction(int.Parse(number));
                    break;
                case CommandType.Forever:
                    action.StartEndlessAction(float.Parse(number));
                    break;
                case CommandType.Conditional:
                    Debug.LogError("Need to make conditional");
                    break;
                case CommandType.Stop:
                    action.StopAllActions();
                    break;
                default:
                    break;
            }
        }
    }

    private CommandType GetCommandType(string command)
    {
        if (command.Contains("stop"))
            return CommandType.Stop;
        else if (command.Contains("times"))
            return CommandType.Iterative;
        else if (command.Contains("when"))
            return CommandType.Conditional;
        else if (command.Contains("every"))
            return CommandType.Forever;

        return CommandType.Manual;
    }
}
