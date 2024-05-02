using UnityEngine;
using Unity;

//Cleans up gameobjects so they are not kept between scenes.
class Cleanuper
{
    const float SECS_BEFORE_DESTROYING = 0.1f;

    // We trust that the object calling this will also set the same input for unloading their scene.
    public static void ReadyCleanUpForSceneTransition(float secsUntilSceneUnloads)
    {
        
        Invoker.InvokeDelayed(() => CleanUpForSceneTransition(), secsUntilSceneUnloads - SECS_BEFORE_DESTROYING); 
    }
    static void CleanUpForSceneTransition()
    {
        var PoofObjs = GameObject.FindObjectsByType<PoofDestroy>(FindObjectsSortMode.InstanceID);
        foreach (var PoofObj in PoofObjs)
        {
            GameObject.Destroy(PoofObj);
        }
    }
}