using UnityEngine;

public static class DayNightSystem
{
    public static float timeOfDay;
    public static int days;
    public const int SECONDS_IN_DAY = 3600; //1 hour of real-time gameplay per day.


    // This is supposed to be called when we are outside.
    public static void UpdateTimeOutside()
    {
        timeOfDay += Time.deltaTime;
        if (timeOfDay > SECONDS_IN_DAY)
        {
            timeOfDay -= SECONDS_IN_DAY;
            days++;
        }
    }
    public static float GetDayProgress()
    {
        return timeOfDay / SECONDS_IN_DAY;
    }
}
