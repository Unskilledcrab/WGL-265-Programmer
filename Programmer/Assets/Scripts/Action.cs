using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Action 
{
    public string[] CommandNames;
    public UnityEvent Event;
    public float IteratorDelay = 1f;
    public float MinimumEndlessDelay = 3f;
        
    private MonoBehaviour _parent;
    private Dictionary<string, IEnumerator> _activeCoroutines = new();
    private string _commands => $"[{string.Join(',', CommandNames)}]";

    public void SetParent(MonoBehaviour monoBehaviour)
    {
        _parent = monoBehaviour;
    }

    public void CallAction([CallerMemberName] string callerName = "")
    {
        Debug.Log($"{_commands}: Calling the action!");
        Event?.Invoke();
    }

    public void StopAction(Func<bool> condition)
    {
        InternalStopAction("condition");
    }
    public void StopAction(int condition)
    {
        InternalStopAction("iterator");
    }
    public void StopAction(float condition)
    {
        InternalStopAction("endless");
    }
    public void StopAllActions()
    {
        Debug.Log($"{_commands}: Stopping All");
        _parent.StopAllCoroutines();
        _activeCoroutines.Clear();
    }
    public void StartConditionAction(Func<bool> condition)
    {
        var coroutine = RunAction(condition);
        InternalStartAction("condition", coroutine);
    }

    public void StartIterativeAction(int iterator)
    {
        var coroutine = RunAction(iterator);
        InternalStartAction("iterator", coroutine);
    }
    public void StartEndlessAction(float seconds)
    {
        var coroutine = RunAction(seconds);
        InternalStartAction("endless", coroutine);
    }

    private void InternalStopAction(string condition)
    {
        var key = $"[{_commands}]{condition}";
        if (_activeCoroutines.TryGetValue(key, out var coroutine))
        {
            _parent.StopCoroutine(coroutine);
            _activeCoroutines.Remove(key);
            Debug.Log($"Stopped: {key}");
        }
    }
    private void InternalStartAction(string condition, IEnumerator coroutine)
    {
        var key = $"[{_commands}]{condition}";
        InternalStopAction(condition);
        _activeCoroutines.TryAdd(key, coroutine);
        _parent.StartCoroutine(coroutine);
        Debug.Log($"Started: {key}");
    }

    private IEnumerator RunAction(Func<bool> condition)
    {
        if (condition())
        {
            CallAction();
        }
        yield return new WaitForSeconds(IteratorDelay);
    }

    private IEnumerator RunAction(int iterator)
    {
        for (int i = 0; i < iterator; i++)
        {
            CallAction();
            yield return new WaitForSeconds(IteratorDelay);
        }
    }

    private IEnumerator RunAction(float seconds)
    {
        if (seconds < MinimumEndlessDelay)
            seconds = MinimumEndlessDelay;

        while (true)
        {
            CallAction();
            yield return new WaitForSeconds(seconds);
        }
    }
}


