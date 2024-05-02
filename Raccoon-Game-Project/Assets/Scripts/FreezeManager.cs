using UnityEngine;
static class FreezeMmanager{
    //Generic for finding specific freezers.
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