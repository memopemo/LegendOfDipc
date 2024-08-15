using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animator2D;

// Be airborn and constantly move. 
public class JumpingPlayerState : IPlayerState
{
    float jumpSecs = 0;
    const int ANIM_JUMP = 2;
    const float DEFAULT_SPEED = 6f;
    const float MAX_JUMP_TIME = 0.5f;
    const float JUMP_HEIGHT_NORMAL = 1;
    const float JUMP_HEIGHT_SUPER = 2;
    int framesGrounded = 0;
    bool tryingToCorrect;
    Vector2Int initialDirection;
    GameObject isIceStompingAndWithWhat;
    public JumpingPlayerState(GameObject createAtEnd = null)
    {
        isIceStompingAndWithWhat = createAtEnd;
    }

    public void OnEnter(PlayerStateManager manager)
    {
        //Debug.Log($"Started Jump: {manager.rigidBody.position}");
        //debugStartJumpPos = manager.rigidBody.position;
        manager.animator.SetAnimation(ANIM_JUMP);
        initialDirection = Vector2Int.RoundToInt(manager.rawInput);
    }

    public void OnLeave(PlayerStateManager manager)
    {
        manager.defaultSpriteRenderer.enabled = true;
        //Debug.Log($"Ended Jump: {manager.rigidBody.position}");
        //Debug.Log($"Distance: {Vector2.Distance(debugStartJumpPos, manager.rigidBody.position)}");
        if(isIceStompingAndWithWhat)
        {
            Object.Instantiate(isIceStompingAndWithWhat, manager.transform.position + (Vector3)(Vector2)manager.directionedObject.direction, Quaternion.identity);
        }
    }

    public void OnUpdate(PlayerStateManager manager)
    {
        
        if(framesGrounded < 3) 
        {
            framesGrounded++;
            return;
        }
        Vector2 actualMovement = manager.rawInput;
        if(tryingToCorrect)
        {
            actualMovement /= 2; //half the speed of movement.
        }
        else
        {
            if (Vector2Int.RoundToInt(manager.rawInput) != Vector2.zero && Vector2Int.RoundToInt(manager.rawInput) != initialDirection)
            { 
                tryingToCorrect = true;
            }
        }
        //Constantly move player
        manager.rigidBody.velocity = actualMovement * (DEFAULT_SPEED + manager.additionalSpeed);

        //Tick jump timer
        jumpSecs += Time.deltaTime;

        // Animate jump sprite in a parabola (TEMP)
        // Classic linear algebra moment.
        // Formula for this:
        // (-4h/t^2)*x^2 + (4h/t)x
        // where h is the max height, and t is the length of the jump (in our case seconds)
        // and no I did not just pull that out of my ass, I used Mathway.
        float jumpHeight = GetJumpHeight();
        float a = -4 * jumpHeight / (MAX_JUMP_TIME * MAX_JUMP_TIME);
        float b = 4 * jumpHeight / MAX_JUMP_TIME;
        manager.height.height = (a * jumpSecs * jumpSecs) + (b * jumpSecs); // y = ax^2 + bx


        //Check if done
        if (jumpSecs > MAX_JUMP_TIME) 
        {
            //Leave
            manager.stateTransitionTimer1 = 10;
            manager.SwitchState(new DefaultPlayerState());
            manager.height.height = 0;
            return;
        }
    }

    public float GetJumpHeight() 
    {
        //TODO: change jump height based on what we have. 
        if (true)
        {
            return JUMP_HEIGHT_SUPER;
        }
        //else return JUMP_HEIGHT_NORMAL;
    }
}
