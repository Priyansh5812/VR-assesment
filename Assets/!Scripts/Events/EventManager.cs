using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class EventManager
{
    // Passes the clue Message
    public static ActionEvent<string> OnClueInspected
    {
        get; private set;
    } = new();
    public static ActionEvent OnRetry
    {
        get; private set;
    } = new();

}

public class ActionEvent
{
    private event Action baseAction;

    public void Invoke() => baseAction?.Invoke();

    public void AddListener(Action action) => baseAction += action;

    public void RemoveListener(Action action) => baseAction -= action;
}
public class ActionEvent<T1>
{
    private event Action<T1> baseAction;

    public void Invoke(T1 value) => baseAction?.Invoke(value);

    public void AddListener(Action<T1> action) => baseAction += action;

    public void RemoveListener(Action<T1> action) => baseAction -= action;


}




