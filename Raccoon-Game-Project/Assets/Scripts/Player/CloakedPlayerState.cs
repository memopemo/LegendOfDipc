using UnityEngine;

public class CloakedPlayerState : IPlayerState
{
    const int CLOAK_IDLE = 33;
    const int CLOAK_WALK = 34;
    const int CLOAK_END = 35;
    public void OnEnter(PlayerStateManager manager)
    {
        manager.animator.SetAnimation(CLOAK_IDLE);
    }

    public void OnLeave(PlayerStateManager manager)
    {
        
    }

    public void OnUpdate(PlayerStateManager manager)
    {
        //exit state transition logic 
        if(manager.animator.currentAnimation == CLOAK_END)
        {
            if(manager.animator.finishedFrames > 0)
            {
                manager.SwitchState(new DefaultPlayerState());
            }
            return;
        }
       
        if(Input.GetButtonUp("Fire3"))
        {
            manager.animator.SetAnimation(CLOAK_END);
            return; //return so animation is not set again.
        }
        
        //normal update logic
        CommonPlayerState.MovePlayerRaw(manager, CommonPlayerState.DEFAULT_SPEED/2);
        CommonPlayerState.UpdateDirection(manager);

        if(manager.rawInput != Vector2.zero)
        {
            manager.animator.SetAnimation(CLOAK_WALK);
        }
        else if(manager.stateTransitionTimer1 == 0)
        {
            manager.animator.SetAnimation(CLOAK_IDLE, 4); //set specific frame to end of loop frame so we dont play the
        }
        CommonPlayerState.CheckWater(manager);
    }
}