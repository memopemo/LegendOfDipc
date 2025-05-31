using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionArea : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.TryGetComponent(out PlayerStateManager player))
        {
            _ = new GameObject("PlayerReflection", typeof(PlayerWaterReflection));

        }
        else if(collider.TryGetComponent(out SpriteRenderer r))
        {
            GameObject WaterReflectionObj = new("ObjReflection", typeof(WaterReflection));
            WaterReflectionObj.transform.parent = collider.transform;
        }

    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out PlayerStateManager player))
        {
            FindFirstObjectByType<PlayerWaterReflection>().Exit();
        }
        else if (collider.TryGetComponent(out WaterReflection waterReflection))
        {
            waterReflection.Exit();
        }
    }
}
