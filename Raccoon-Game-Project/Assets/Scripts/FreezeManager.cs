using UnityEngine;
static class FreezeManager
{

    //Generic for finding specific freezers.
    //"For every Freezer Gameobject..."
    public static void FreezeAll<F>() where F : Freezer
    {
        var a = GameObject.FindObjectsByType<F>(FindObjectsInactive.Exclude, FindObjectsSortMode.InstanceID);
        foreach (var item in a)
        {
            item.FreezeChildrenAndSelf();
        }
    }
    public static void UnfreezeAll<F>() where F : Freezer
    {
        var a = GameObject.FindObjectsByType<F>(FindObjectsInactive.Exclude, FindObjectsSortMode.InstanceID);
        foreach (var item in a)
        {
            item.Unfreeze();
        }
    }


}