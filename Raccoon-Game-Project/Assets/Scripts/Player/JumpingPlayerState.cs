using UnityEngine;

// Be airborn and constantly move. 
public class JumpingPlayerState : IPlayerState
{
    float jumpSecs = 0;
    const int ANIM_JUMP = 2;
    const float DEFAULT_SPEED = 6f;
    const float MAX_JUMP_TIME = 0.3f;
    const float JUMP_TIME_MULTIPLIER = 0.175f; //this *HAS* to be particular for jumping across distances to show the impossibility/posibility between 2 unit and 3 unit gaps!!!
    int framesGrounded = 0;
    bool tryingToCorrect;
    Vector2Int initialDirection;
    GameObject isIceStompingAndWithWhat;
    float actualMaxJumpTime;
    public JumpingPlayerState(GameObject createAtEnd = null)
    {
        isIceStompingAndWithWhat = createAtEnd;
    }

    public void OnEnter(PlayerStateManager manager)
    {
        //Debug.Log($"Started Jump: {manager.rigidBody.position}");
        //debugStartJumpPos = manager.rigidBody.position;
        // if(CommonPlayerState.GetJumpHeight() == 0)
        // {
        //     manager.SwitchState(new DefaultPlayerState());
        //     return;
        // }
        //we'll allow the player to do a puny jump (crosses 1 block gaps but thats it) if they have no boots.
        manager.animator.SetAnimation(ANIM_JUMP);
        initialDirection = Vector2Int.RoundToInt(manager.rawInput);
        actualMaxJumpTime = MAX_JUMP_TIME + (JUMP_TIME_MULTIPLIER * CommonPlayerState.GetJumpHeight()); //0 = 0.3f, 1 = 0.475, 2 = 0.65;
    }

    public void OnLeave(PlayerStateManager manager)
    {
        manager.defaultSpriteRenderer.enabled = true;
        //Debug.Log($"Ended Jump: {manager.rigidBody.position}");
        //Debug.Log($"Distance: {Vector2.Distance(debugStartJumpPos, manager.rigidBody.position)}");
        if (isIceStompingAndWithWhat)
        {
            Object.Instantiate(isIceStompingAndWithWhat, manager.transform.position + (Vector3)(Vector2)manager.directionedObject.direction, Quaternion.identity);
        }
    }

    public void OnUpdate(PlayerStateManager manager)
    {

        if (framesGrounded < 3)
        {
            framesGrounded++;
            return;
        }
        Vector2 actualMovement = manager.rawInput;
        if (tryingToCorrect)
        {
            actualMovement /= 2;
        }
        else
        {
            if (Vector2Int.RoundToInt(manager.rawInput) != Vector2.zero && Vector2Int.RoundToInt(manager.rawInput) != initialDirection)
            {
                tryingToCorrect = true;
            }
        }
        //Constantly move player
        manager.rigidBody.linearVelocity = actualMovement * (DEFAULT_SPEED + manager.additionalSpeed);

        //Tick jump timer
        jumpSecs += Time.deltaTime;

        // Animate jump sprite in a parabola (TEMP)
        // Classic linear algebra moment.
        // Formula for this:
        // (-4h/t^2)*x^2 + (4h/t)x
        // where h is the max height, and t is the length of the jump (in our case seconds)
        // and no I did not just pull that out of my ass, I used Mathway.
        float jumpHeight = CommonPlayerState.GetJumpHeight() + 1f;
        float parabolaWidth = -4 * jumpHeight / (actualMaxJumpTime * actualMaxJumpTime);
        float slopeAtStart = 4 * jumpHeight / actualMaxJumpTime;
        manager.height.height = (parabolaWidth * jumpSecs * jumpSecs) + (slopeAtStart * jumpSecs);


        //Check if done
        if (jumpSecs > actualMaxJumpTime)
        {
            //Leave
            manager.stateTransitionTimer1 = 10;
            manager.SwitchState(new DefaultPlayerState());
            manager.height.height = 0;
            return;
        }
    }
}
