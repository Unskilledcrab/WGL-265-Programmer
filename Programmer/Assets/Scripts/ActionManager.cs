using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ActionManager : MonoBehaviour
{
    public TMPro.TMP_InputField InputField;
    public Action[] Actions;

    private Dictionary<string, Action> _actions = new();
    private List<string> _previousCommands = new();
    private int _index = 0;

    private void Start()
    {
        foreach (var action in Actions)
        {
            action.SetParent(this);
            foreach (var commandName in action.CommandNames)
            {
                if (!_actions.TryAdd(commandName.ToLower(), action))
                    Debug.LogError($"The command name [{commandName}] is already taken");
            }
        }
        InputField.ActivateInputField();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ParseInput();
            _index = _previousCommands.Count > 0 ? _previousCommands.Count - 1 : 0;
            InputField.text = string.Empty;
            InputField.ActivateInputField();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (_previousCommands.Any())
            {
                InputField.text = _previousCommands[_index];
                if (_index != 0)
                {
                    _index--;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (_previousCommands.Any())
            {
                InputField.text = _previousCommands[_index];
                if (_index != _previousCommands.Count - 1)
                {
                    _index++;
                }
            }
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
            _previousCommands.Add(command);
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
                    action.CallAction();
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
        _previousCommands.Add(command);
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
