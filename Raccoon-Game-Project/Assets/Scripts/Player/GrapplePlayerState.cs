using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplePlayerState : IPlayerState
{
    const int GRAPPLE_ANIM = 28; 
    
    public GrapplePlayerState(PlayerStateManager manager, Transform tapeMeasurer)
    {
        manager.transform.parent = tapeMeasurer;
    }
    public void OnEnter(PlayerStateManager manager)
    {
        manager.animator.SetAnimation(GRAPPLE_ANIM);
        manager.rigidBody.linearVelocity = Vector2.zero;
    }

    public void OnLeave(PlayerStateManager manager)
    {
        manager.transform.parent = null;
    }

    public void OnUpdate(PlayerStateManager manager)
    {
    }
    public void StartDragAnimation(PlayerStateManager manager)
    {
        manager.animator.SetAnimation(GRAPPLE_ANIM+1);
    }
}
