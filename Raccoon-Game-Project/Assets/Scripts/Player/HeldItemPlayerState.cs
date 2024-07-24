using UnityEngine;

class HeldItemPlayerState : IPlayerState
{
    const float SPEED = 4;
    const int HOLD_ENTER = 23;
    const int HOLD_IDLE = 24;
    const int HOLD_WALK = 25;
    const int HOLD_EXIT = 26;
    const int SHOOT = 27;

    const int HOLD_ENTER_LENGTH_FRAMES = 20;
    const int HOLD_EXIT_LENGTH_FRAMES = 10;
    const int SHOOT_LENGTH_FRAMES = 10;


    /** Some info:
     *  Transition Timer 1 is for exiting
     * 2 is for exiting
     * GeneralTimer1 is for kickback animation.
     */
   
    
    public void OnEnter(PlayerStateManager manager)
    {
        manager.stateTransitionTimer1 = HOLD_ENTER_LENGTH_FRAMES; //enter timer
        manager.stateTransitionTimer2 = -1; //exit timer
        manager.animator.SetAnimation(HOLD_ENTER); //set state
    }

    public void OnLeave(PlayerStateManager manager)
    {
    }

    public void OnUpdate(PlayerStateManager manager)
    {
        //dont move on enter or exit
        if(manager.stateTransitionTimer1 > 0) //is enter active
        {
            return;
        }
        if(manager.stateTransitionTimer2 > 0)
        {
            manager.animator.SetAnimation(HOLD_EXIT);
            return;
        }
        else if(manager.stateTransitionTimer2 == 0)
        {
            manager.SwitchState(new DefaultPlayerState());
            GameObject.Destroy(manager.generalGO);
        }

        if(manager.generalGO == null) //is all used up?
        {
            manager.SwitchState(new DefaultPlayerState());
            manager.animator.SetAnimation(0);
            return;
        }

        // Not interrupted by transition animations. commence normal ops
        CommonPlayerState.MovePlayerRaw(manager, SPEED);

        //play kick back animation
        if(manager.generalTimer1 > 0)
        {
            manager.animator.SetAnimation(SHOOT);
            return;
        }
         //play regular animation depending if we are walking or moving 
        manager.animator.SetAnimation(manager.rigidBody.velocity == Vector2.zero ? HOLD_IDLE : HOLD_WALK);

        //Do not change directions.
    }
    public void Kickback(PlayerStateManager manager)
    {
        manager.generalTimer1 = SHOOT_LENGTH_FRAMES;
        manager.rigidBody.AddForce(manager.directionedObject.direction * -10, ForceMode2D.Force);
    }
    public void ExitUsed(PlayerStateManager manager)
    {
        manager.SwitchState(new DefaultPlayerState());
    }
    public void ExitCanceled(PlayerStateManager manager)
    {
        manager.stateTransitionTimer2 = HOLD_EXIT_LENGTH_FRAMES;
    }
}