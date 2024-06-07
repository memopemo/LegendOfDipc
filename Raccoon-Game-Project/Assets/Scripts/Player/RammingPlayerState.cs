using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Dash forward and move a little bit until you crash into something.
public class RammingPlayerState : IPlayerState
{
    const float WINDUP_SECS = 0.5f;
    float windupTime;
    const int WINDUP_ANIM = 0;
    const int DASH_ANIM = 0;
    const float DASH_SPEED = CommonPlayerState.DEFAULT_SPEED + 2;
    

    public void OnEnter(PlayerStateManager manager)
    {
        manager.animator.SetAnimation(WINDUP_ANIM);
    }
    public void OnLeave(PlayerStateManager manager)
    {
    }
    public void OnUpdate(PlayerStateManager manager)
    {
        windupTime += Time.deltaTime;
        if(windupTime < WINDUP_SECS)
        {
            return;
        }
        else
        {
            manager.animator.SetAnimation(DASH_ANIM);
            manager.rigidBody.velocity = (Vector2)manager.directionedObject.direction * ( DASH_SPEED + manager.additionalSpeed);
        }
    }
}
