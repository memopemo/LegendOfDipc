using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnorkelPlayerState : IPlayerState
{
    const float SNORKEL_SPEED = 6f;
    public void OnEnter(PlayerStateManager manager)
    {
    }
    public void OnLeave(PlayerStateManager manager)
    {
    }
    public void OnUpdate(PlayerStateManager manager)
    {
        CommonPlayerState.MovePlayerSmooth(manager, SNORKEL_SPEED);
        CommonPlayerState.UpdateDirection(manager);
    }
}
