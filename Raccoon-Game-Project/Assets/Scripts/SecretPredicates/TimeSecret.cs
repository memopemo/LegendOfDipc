public class TimeSecret : SecretPredicate
{
    public int timeStart;
    public int timeEnd;
    public override string Evaluate() //output is either empty or gives a reason.
    {
        if (1 > timeStart && 1 < timeEnd)
        {
            return "";
        }
        else
        {
            return $"Time is outside of range: [{timeStart}, {timeEnd}] given: {1}";
        }
    }
}
