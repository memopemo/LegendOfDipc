using System;
using UnityEngine;

public static class DayNightSystem
{
    public const int SECONDS_IN_DAY = 3600; //1 hour of real-time gameplay per day.
    [Serializable]
    public enum TimesOfDay {DAY=0, DUSK=SECONDS_IN_DAY/4, DNIGHT = SECONDS_IN_DAY/2, DAWN=3*SECONDS_IN_DAY/4}

    // This is supposed to be called when we are outside.
    public static void UpdateTimeOutside()
    {
        SaveFile sf = SaveManager.GetSave();
        sf.TimeOfDay += Time.deltaTime;
        if (sf.TimeOfDay > SECONDS_IN_DAY)
        {
            sf.TimeOfDay -= SECONDS_IN_DAY;
            sf.Days++;
        }
    }
    public static void AddTime(float secs)
    {
        SaveFile sf = SaveManager.GetSave();
        sf.TimeOfDay += secs;
    }
    public static void SetTime(TimesOfDay timesOfDay)
    {
        SaveFile sf = SaveManager.GetSave();
        sf.TimeOfDay = (int)timesOfDay;
    }
    public static float GetDayProgress()
    {
        SaveFile sf = SaveManager.GetSave();
        return sf.TimeOfDay / SECONDS_IN_DAY;
    }
}
