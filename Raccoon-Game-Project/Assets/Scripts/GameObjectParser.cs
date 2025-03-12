using System.Text.RegularExpressions;
using UnityEngine;
public static class GameObjectParser
{
    public static int GetIndexFromName(GameObject go)
    {
        return GetIndexFromString(go.name);
    }
    public static int GetIndexFromString(string str)
    {
        try
        {
            return int.Parse(Regex.Match(str, @"\d+$", RegexOptions.RightToLeft).Value); //dont ask, copied from internet
        }
        catch (System.Exception)
        {
            return 0;
        }
    }

}