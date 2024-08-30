using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPlayerState : IPlayerState
{
    const int THROW_ANIM = 2; //replace
    const float SPEED = CommonPlayerState.DEFAULT_SPEED-2;
    DamagesPlayer currentlyTouchingEnemy;
    public void OnEnter(PlayerStateManager manager)
    {
        manager.animator.SetAnimation(1);
    }

    public void OnLeave(PlayerStateManager manager)
    {
        
    }

    public void OnUpdate(PlayerStateManager manager)
    {
        CommonPlayerState.MovePlayerRaw(manager, SPEED);
        if(!Input.GetButton("Shield"))
        {
            //are we still touching when leaving?
            if(currentlyTouchingEnemy && manager.rigidBody.IsTouching(currentlyTouchingEnemy.GetComponent<Collider2D>()))
            {
                //take damage if so.
                manager.TakeDamage(currentlyTouchingEnemy);
                return;
            }
            manager.SwitchState(new DefaultPlayerState());
        }
    }
    public void AddDamagesPlayer(DamagesPlayer hurtful)
    {
        currentlyTouchingEnemy = hurtful;
    }
}