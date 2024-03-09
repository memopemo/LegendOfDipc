using System.Text.RegularExpressions;
using UnityEngine;

public static class Common
{
    public static Vector3 SnapToGrid(Vector3 v)
    {
        v += (Vector3)Vector2.one * 0.5f;
        v = new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
        v -= (Vector3)Vector2.one * 0.5f;
        return v;
    }
    public static Vector2 SnapToGrid(Vector2 v)
    {
        v += Vector2.one * 0.5f;
        v = new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
        v -= Vector2.one * 0.5f;
        return v;
    }
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