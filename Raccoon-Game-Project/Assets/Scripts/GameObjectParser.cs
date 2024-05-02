using System.Text.RegularExpressions;
using UnityEngine;

public static class GameObjectParser
{
    public static int GetIndexFromName(GameObject go)
    {
        try
        {
            return int.Parse(Regex.Match(go.name, @"\d+$", RegexOptions.RightToLeft).Value); //dont ask, copied from internet
        }
        catch (System.Exception)
        {
            return 0;
        }

    }

}