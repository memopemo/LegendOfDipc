using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Cutscenes. Must be exited at some point!
public class NoInputPlayerState : IPlayerState
{
    public void OnEnter(PlayerStateManager manager)
    {
        manager.SetAnimation(0);
    }

    public void OnLeave(PlayerStateManager manager){}

    public void OnUpdate(PlayerStateManager manager){}
}
