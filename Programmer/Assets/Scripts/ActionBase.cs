using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public abstract class ActionBase : MonoBehaviour
{
    public string[] CommandNames;
    public UnityEvent Event;
    public const float DefaultDelay = 1f;

    private Dictionary<string, IEnumerator> ActiveCoroutines = new();
    private string _commands => $"[{string.Join(',', CommandNames)}]";

    public void Action([CallerMemberName] string callerName = "")
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
        StopAllCoroutines();
        ActiveCoroutines.Clear();
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
    public void StartEndlessAction(float seconds = DefaultDelay)
    {
        Debug.Log($"{_commands}: Calling endless {seconds} second delay");
        var coroutine = RunAction(seconds);
        InternalStartAction(seconds.ToString(), coroutine);
    }

    private void InternalStopAction(object key)
    {
        if (ActiveCoroutines.TryGetValue(key.ToString(), out var coroutine))
        {
            StopCoroutine(coroutine);
            ActiveCoroutines.Remove(key.ToString());
        }
    }
    private void InternalStartAction(string condition, IEnumerator coroutine)
    {
        InternalStopAction(condition);
        ActiveCoroutines.TryAdd(condition.ToString(), coroutine);
        StartCoroutine(coroutine);
    }

    private IEnumerator RunAction(Func<bool> condition)
    {
        if (condition())
        {
            Action();
        }
        yield return new WaitForSeconds(DefaultDelay);
    }

    private IEnumerator RunAction(int iterator)
    {
        for (int i = 0; i < iterator; i++)
        {
            Action();
            yield return new WaitForSeconds(DefaultDelay);
        }
    }

    private IEnumerator RunAction(float seconds = DefaultDelay)
    {
        while (true)
        {
            Action();
            yield return new WaitForSeconds(seconds);
        }
    }
}


