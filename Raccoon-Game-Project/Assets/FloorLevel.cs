using System.Collections.Generic;
using UnityEngine;

public class FloorLevel : MonoBehaviour
{
    public static Dictionary<Level, int> levelToUnityLayers = new()
        {
            { Level.Bottom,     6},
            { Level.Middle,     7},
            { Level.Top,        8},
            { Level.Ceiling,    9},
        };
    //This is used by heightable to add its z value by whatever the height is.

    public static Dictionary<Level, int> levelToZOffset = new()
        {
            { Level.Bottom,     0},
            { Level.Middle,     3},
            { Level.Top,        6},
            { Level.Ceiling,    9},
        };
    public enum Level { Bottom, Middle, Top, Ceiling, }

    [SerializeField]
    private Level currentlevel;

    public void IncrementLevel(int amount = 1)
    {
        currentlevel += amount;
        if (currentlevel > Level.Ceiling)
        {
            currentlevel = Level.Ceiling;
        }
    }
    public void DecrementLevel(int amount = 1)
    {
        currentlevel -= amount;
        if (currentlevel < Level.Bottom)
        {
            currentlevel = Level.Bottom;
        }
    }
    private void Update()
    {
        SetUnityLayer();
    }
    private void SetUnityLayer()
    {
        levelToUnityLayers.TryGetValue(currentlevel, out int i);
        gameObject.layer = i;
    }
    public int GetZOffset()
    {
        // Our Dictionary contains all possible enums, so we dont need to worry about if its right or not.
        levelToZOffset.TryGetValue(currentlevel, out int i);
        return i;
    }

}
