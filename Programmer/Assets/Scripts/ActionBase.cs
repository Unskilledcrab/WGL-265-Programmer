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
        InternalStopAction(condition);
    }
    public void StopAction(int condition)
    {
        InternalStopAction(condition);
    }
    public void StopAction(float condition)
    {
        InternalStopAction(condition);
    }
    public void StopAllActions()
    {
        Debug.Log($"{_commands}: Stopping");
        _parent.StopAllCoroutines();
        _activeCoroutines.Clear();
    }
    public void StartConditionAction(Func<bool> condition)
    {
        Debug.Log($"{_commands}: Calling condition {condition}");
        var coroutine = RunAction(condition);
        InternalStartAction(condition.ToString(), coroutine);
    }

    public void StartIterativeAction(int iterator)
    {
        Debug.Log($"{_commands}: Calling iterator {iterator} times");
        var coroutine = RunAction(iterator);
        InternalStartAction(iterator.ToString(), coroutine);
    }
    public void StartEndlessAction(float seconds)
    {
        Debug.Log($"{_commands}: Calling endless {seconds} second delay");
        var coroutine = RunAction(seconds);
        InternalStartAction(seconds.ToString(), coroutine);
    }

    private void InternalStopAction(object key)
    {
        if (_activeCoroutines.TryGetValue(key.ToString(), out var coroutine))
        {
            _parent.StopCoroutine(coroutine);
            _activeCoroutines.Remove(key.ToString());
        }
    }
    private void InternalStartAction(string condition, IEnumerator coroutine)
    {
        InternalStopAction(condition);
        _activeCoroutines.TryAdd(condition.ToString(), coroutine);
        _parent.StartCoroutine(coroutine);
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
        while (true)
        {
            CallAction();
            yield return new WaitForSeconds(seconds);
        }
    }
}


