using System;
using UnityEngine;
using UnityEngine.Events;
//Tag for any object that is grabbable.
public class Grabbable : MonoBehaviour
{
    int cooldownTimer;
    const int COOLDOWN_TIMER_SET = 60;
    public UnityEvent OnGrab;
    public UnityEvent<Vector2> OnPush;
    public UnityEvent<Vector2> OnPull;
    public UnityEvent<Vector2> OnStartPush;
    public UnityEvent<Vector2> OnStartPull;
    public UnityEvent<Vector2> OnEndPush;
    public UnityEvent<Vector2> OnEndPull;
    public UnityEvent OnUngrab;

    public void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer--;
        }
    }
    public void Grab()
    {
        OnGrab.Invoke();
    }
    public void Push(Vector2 on)
    {
        if (cooldownTimer != 0) return;
        OnPush.Invoke(on);
        cooldownTimer = COOLDOWN_TIMER_SET;

    }
    public void Pull(Vector2 on)
    {
        if (cooldownTimer != 0) return;
        OnPull.Invoke(on);
        cooldownTimer = COOLDOWN_TIMER_SET;

    }
    public void StartPush(Vector2 on)
    {
        if (cooldownTimer != 0) return;
        OnStartPush.Invoke(on);
    }
    public void StartPull(Vector2 on)
    {
        if (cooldownTimer != 0) return;
        OnStartPull.Invoke(on);
    }
    public void EndPush(Vector2 on)
    {
        OnEndPush.Invoke(on);
    }
    public void EndPull(Vector2 on)
    {
        OnEndPull.Invoke(on);
    }
    public void Ungrab()
    {
        OnUngrab.Invoke();
    }
}
