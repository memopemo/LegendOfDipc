using System.Collections.Generic;
using UnityEngine;

public class SwimPlayerState : IPlayerState
{
    const int SWIM_SPEED = 11;
    Vector2 divePosition = Vector2.zero;
    public void OnEnter(PlayerStateManager manager)
    {
        manager.animator.SetAnimation(8);
        Object.Instantiate(manager.splashParticle, manager.transform.position, manager.transform.rotation);
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

        //if touching water, return from function.

        ContactFilter2D contactFilter = new()
        {
            layerMask = LayerMask.NameToLayer("Default"),
            useTriggers = true,
            minDepth = manager.transform.position.z,
            maxDepth = manager.transform.position.z + 3
        };
        List<RaycastHit2D> results = new();
        int _ = Physics2D.BoxCast(manager.transform.position + Vector3.down * 0.2f, Vector2.one * 0.01f, 0, Vector2.zero, contactFilter, results, 0);
        if (Input.GetButtonDown("Fire1"))
        {
            foreach (RaycastHit2D hit in results)
            {

                if (hit.collider.TryGetComponent(out DeepWater _))
                {
                    manager.DiveIntoDeepWater();
                    return;
                }
                Debug.Log("Nope. Not deepwater.");
            }
        }
        foreach (RaycastHit2D hit in results)
        {
            if (hit.collider.TryGetComponent(out Water _))
            {
                // Update will usually end here.
                return;
            }
        }
        // The only way we would be able to hit this is if there was no water at all. Thus, we switch back to default. :)
        // SMARTS!
        manager.SwitchState(new DefaultPlayerState());

    }
}
