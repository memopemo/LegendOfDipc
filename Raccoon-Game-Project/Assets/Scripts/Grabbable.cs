using UnityEngine;
using UnityEngine.Events;
//Tag for any object that is grabbable.
public class Grabbable : MonoBehaviour 
{
    int cooldownTimer;
    const int COOLDOWN_TIMER_SET = 60;
    [SerializeField] UnityEvent<Vector2> OnPush;
    [SerializeField] UnityEvent<Vector2> OnPull;

    public void Update()
    {
        if(cooldownTimer > 0)
        {
            cooldownTimer--;
        }
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
}
