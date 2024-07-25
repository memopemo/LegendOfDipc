#if DEBUG
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Move without bounds.
public class DebugFreeRoamPlayerState : IPlayerState
{
    float speed = 1;
    public void OnEnter(PlayerStateManager manager)
    {
        manager.rigidBody.simulated = false;
    }

    public void OnLeave(PlayerStateManager manager)
    {
        manager.rigidBody.simulated = true;
    }

    public void OnUpdate(PlayerStateManager manager)
    {
        if (!Debug.isDebugBuild) return; //failsafe
        manager.transform.position += (Vector3)manager.rawInput*speed;
        if (Input.GetKeyDown(KeyCode.PageUp)) 
        {
            speed *= 2;
        }
        else if (Input.GetKeyDown(KeyCode.PageDown))
        {
            speed /= 2;
        }
    }
}
#endif