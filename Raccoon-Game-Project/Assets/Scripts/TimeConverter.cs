using System.Text;
using UnityEngine;

public static class TimeConverter
{
    //break up float seconds into piecemeal hours, mins, seconds, and milliseconds.
    public static string SecsToDisplayMinSecs(float totalSeconds, bool showHour = false, bool showMinute = true, bool showSecond = true, bool showMillisecond = false)
    {
        int roundedSeconds = Mathf.FloorToInt(totalSeconds);
        int hrs = roundedSeconds / 3600;
        int mins = roundedSeconds / 60;
        int secs = roundedSeconds % 60;
        int ms = Mathf.FloorToInt(totalSeconds * 1000) % 1000;
        StringBuilder stringBuilder= new StringBuilder();
        if(showHour)
        {
            if(hrs < 10)
                stringBuilder.Append("0");
            stringBuilder.Append(hrs);
        }
        if(showMinute)
        {
            if(stringBuilder.Length != 0)
                stringBuilder.Append(":");
            if(mins < 10)
                stringBuilder.Append("0");
            stringBuilder.Append(mins);
        }
        if(showSecond)
        {
            if(stringBuilder.Length != 0)
                stringBuilder.Append(":");
            if(secs < 10)
                stringBuilder.Append("0");
            stringBuilder.Append(secs);
        }
        if(showMillisecond)
        {
            if(stringBuilder.Length != 0)
                stringBuilder.Append(":");
            stringBuilder.Append(ms);
        }
        return stringBuilder.ToString();
    }
}