using UnityEngine;

public class HoverPlayerState : IPlayerState
{
    public enum HoverType { Umbrella, Feather }
    const int ANIM_JUMP = 2;
    HoverType hoverType;
    bool isHovering;
    float jumpSecs;
    float secs;
    const float MAX_JUMP_TIME = 0.5f;
    const float MAX_FLOAT_TIME = 3f;
    const float MOVEMENT_SPEED = 6f;

    public HoverPlayerState(HoverType hoverType)
    {
        this.hoverType = hoverType;
    }
    public void OnEnter(PlayerStateManager manager)
    {
        if (CommonPlayerState.GetJumpHeight() == 0)
        {
            manager.SwitchState(new DefaultPlayerState());
            return;
        }
        manager.animator.SetAnimation(ANIM_JUMP);
    }

    public void OnLeave(PlayerStateManager manager)
    {
    }

    public void OnUpdate(PlayerStateManager manager)
    {
        if (jumpSecs < MAX_JUMP_TIME / 2 && jumpSecs + Time.deltaTime > MAX_JUMP_TIME / 2) //switching from positive to negative velocity
        {
            isHovering = true;
        }

        if (isHovering && (secs > MAX_FLOAT_TIME || !Buttons.IsButtonHeld(Buttons.KeyItem))) //hover to falling
        {
            isHovering = false;
        }

        if (!isHovering)
        {
            jumpSecs += Time.deltaTime; //increment jump position
        }
        else
        {
            secs += Time.deltaTime;// increment float seconds
            if (hoverType == HoverType.Umbrella)
            {
                jumpSecs += Time.deltaTime / 10; //slowly fall down if umbrella
            }
        }

        //controls
        if (hoverType == HoverType.Feather)
        {
            CommonPlayerState.MovePlayerRaw(manager, MOVEMENT_SPEED); //hover feather allows all directional movement.
        }
        else //umbrella only allows movement in the axis of the entering direction.
        {
            manager.rigidBody.AddForce(manager.rawInput * new Vector2(Mathf.Abs(manager.directionedObject.direction.x),
                                                                        Mathf.Abs(manager.directionedObject.direction.y)) * MOVEMENT_SPEED * Time.deltaTime * 120);
        }

        float jumpHeight = CommonPlayerState.GetJumpHeight();
        float a = -4 * jumpHeight / (MAX_JUMP_TIME * MAX_JUMP_TIME);
        float b = 4 * jumpHeight / MAX_JUMP_TIME;
        manager.height.height = (a * jumpSecs * jumpSecs) + (b * jumpSecs); // y = ax^2 + bx

        if (jumpSecs > MAX_JUMP_TIME)
        {
            //Leave
            manager.stateTransitionTimer1 = 10;
            manager.SwitchState(new DefaultPlayerState());
            manager.height.height = 0;
            return;
        }
    }
}