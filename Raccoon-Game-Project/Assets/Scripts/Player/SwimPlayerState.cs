using System.Collections.Generic;
using UnityEngine;

public class SwimPlayerState : IPlayerState
{
    const int SWIM_SPEED = 11;
    const int STEAM_PARTICLE_INDEX = 2;
    Vector2 divePosition = Vector2.zero;
    CollisionCheck waterCheck;
    public void OnEnter(PlayerStateManager manager)
    {
        manager.animator.SetAnimation(8);
        Object.Instantiate(manager.splashParticle, manager.transform.position, manager.transform.rotation);
        Status status = manager.GetComponent<Status>();
        if (status.statusTicks[(int)Status.Effect.Fire] != 0)
        {
            status.ClearStatus(Status.Effect.Fire);
            manager.GetComponent<ParticleMaker>().CreateParticle(STEAM_PARTICLE_INDEX);
        }
        waterCheck = new(manager.GetComponent<Collider2D>());
        waterCheck
            .SetFindTriggers(true)
            .SetRelativePosition(Vector3.down * 0.2f)
            .SetBoxSize(Vector2.one * 0.01f)
            .SetZRange(manager.transform.position.z, manager.transform.position.z + 3)
            .SetCollisionLayer(LayerMask.NameToLayer("Default"))
            .SetType(CollisionCheck.CollisionType.Point)
            .SetDebug(true);

    }
    public void OnLeave(PlayerStateManager manager)
    {
    }
    public void OnUpdate(PlayerStateManager manager)
    {
        CommonPlayerState.MovePlayerSmooth(manager, SWIM_SPEED);

        CommonPlayerState.UpdateDirection(manager);


        if (manager.rawInput != Vector2.zero)
        {
            manager.animator.SetAnimation(7);
        }
        else
        {
            if (manager.animator.currentAnimation == 7 && manager.animator.currentAnimationFrame == manager.animator.animations[manager.animator.currentAnimation].LoopStart)
            {
                manager.animator.SetAnimation(8);
            }
            else if (manager.animator.currentAnimation == 8)
            {
                manager.animator.SetAnimation(8);
            }
        }

        if (Buttons.IsButtonDown(Buttons.Sword) && SaveManager.GetSave().ObtainedKeyUnselectableItems[0])
        {
            waterCheck.Evaluate<DeepWater>((_) => manager.DiveIntoDeepWater());
        }
        else if (!waterCheck.Evaluate<Water>((_) => { }))
        {
            manager.SwitchState(new DefaultPlayerState());
        }
    }
}
