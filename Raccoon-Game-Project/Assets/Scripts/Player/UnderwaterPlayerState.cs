using System;
using UnityEngine;

public class UnderwaterPlayerState : IPlayerState
{
    float speed;
    const float DEFAULT_SPEED = 10;
    float verticalSpeed;
    public void OnEnter(PlayerStateManager manager)
    {
    }

    public void OnLeave(PlayerStateManager manager)
    {
    }

    public void OnUpdate(PlayerStateManager manager)
    {
        CommonPlayerState.MovePlayerSmooth(manager, speed);
        CommonPlayerState.UpdateDirection(manager);

        if (Buttons.IsButtonDown(Buttons.Sword) && speed == DEFAULT_SPEED)
        {
            speed = 20;
        }
        speed -= Time.deltaTime * 10;
        verticalSpeed -= Time.deltaTime * 8;
        if (speed < DEFAULT_SPEED)
        {
            speed = DEFAULT_SPEED;
        }
        if (Buttons.IsButtonDown(Buttons.Jump))
        {
            verticalSpeed += 5;
        }

        verticalSpeed = Mathf.Clamp(verticalSpeed, -4, 6);
        manager.height.height += verticalSpeed * Time.deltaTime;
        if (manager.height.height < 0)
        {
            manager.height.height = 0;
            verticalSpeed = 0;
        }
        CollisionCheck exitCheck = new(manager.GetComponent<Collider2D>());
        exitCheck
        .SetType(CollisionCheck.CollisionType.Point)
        .SetFindTriggers(true);
        if (manager.height.height > 10)
        {
            exitCheck.Evaluate<Surface>((_) => { manager.RiseToSurface(); });
        }

    }
}
