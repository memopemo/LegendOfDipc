using UnityEngine;
public static class Timer
{
    public static void DecrementTimer(ref int timer)
    {
        if (timer > 0)
        {
            timer--;
        }
    }
    public static void DecrementTimer(ref float timer)
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

    }

}
