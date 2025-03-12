using UnityEngine;

//TODO:
// Add support for jumping off sideways and off the back in dungeons from various levels.

public class JumpingLedgePlayerState : IPlayerState
{
    /* What kind of ledge we are dealing with.
     * 0: Down, 1: 1 Up, 2: 2 Up,
     * 3+?? Maybe??? if we want to increase how high we jump??
     * -1 for Jumping Off Top.
     * -2 for Jumping Off Sides.
     */
    int LedgeType;
    float jumpSex;

    // Y position of bottom ledge.
    float BottomLedgeY;

    int DecrementLevel;

    /* This is our velocity for jumping down from a ledge.
     * It starts negative to give a small jump up before falling down.
     * Otherwise it would look weird to just see him immediately accelerate downwards.
     */
    float downVelocity = -5;

    // Jump timer
    float jumpUpTimer = 0;
    const int ANIM_JUMP = 2;

    const float DOWN_MAX_VELOCITY = 15;
    private const int DOWN_ACCELERATION = 30; //
    private const float UP_TIMESCALE = 1.25f;
    const float UP_TOTAL_TIME = 0.75f;

    //This is because landing *directly at the ledge end* looks off.
    const float UP_LANDING_OFFSET = 0.5f;


    /* Original Y position for starting to jump up.
     * We use this so that the player doesnt start *litterally from 0,0* and then jump up.
     */
    float originalY;

    // We use a custom constructor to tell how high the ledge is, and where the bottom of the ledge is for landing.
    public JumpingLedgePlayerState(int ledgeMoment, float ledgeY, int decrementLevel = -1)
    {
        LedgeType = ledgeMoment;
        BottomLedgeY = ledgeY;
        DecrementLevel = decrementLevel;

    }

    public void OnEnter(PlayerStateManager manager)
    {
        manager.animator.SetAnimation(ANIM_JUMP);
        //Disable rigidbody and keep it still.
        manager.rigidBody.bodyType = RigidbodyType2D.Kinematic;
        manager.rigidBody.linearVelocity = Vector2.zero;

        originalY = manager.transform.position.y;

        // Create a big jump particle if we jump up by 2 for extra Oomph!
        if (LedgeType == 2)
        {
            Object.Instantiate(manager.poofParticle, manager.transform.position, Quaternion.identity);
        }
        if (manager.TryGetComponent(out FloorLevel fl) && DecrementLevel != -1 && LedgeType > 0)
        {
            fl.IncrementLevel(DecrementLevel);
        }
    }

    public void OnLeave(PlayerStateManager manager)
    {
        // Enable rigidbody still keep it still.
        manager.rigidBody.bodyType = RigidbodyType2D.Dynamic;
        manager.rigidBody.linearVelocity = Vector2.zero;

        //we separate increment and decrement to maintain a good z value (so we dont look like we are underneath anything.
        if (manager.TryGetComponent(out FloorLevel fl) && DecrementLevel != -1 && LedgeType == -1)
        {
            fl.DecrementLevel(DecrementLevel);
        }
    }

    public void OnUpdate(PlayerStateManager manager)
    {
        switch (LedgeType)
        {
            case -2:
            case -1:
                const float DEFAULT_SPEED = 1f;
                const float MAX_JUMP_TIME = 0.5f;
                //Constantly move player
                manager.rigidBody.linearVelocity = (Vector2)manager.directionedObject.direction * (DEFAULT_SPEED + manager.additionalSpeed);

                //Tick jump timer
                jumpSex += Time.deltaTime;

                // Animate jump sprite in a parabola (TEMP)
                // Classic linear algebra moment.
                // Formula for this:
                // (-4h/t^2)*x^2 + (4h/t)x
                // where h is the max height, and t is the length of the jump (in our case seconds)
                // and no I did not just pull that out of my ass, I used Mathway.
                float jumpHeight = 1;
                float aa = -4 * jumpHeight / (MAX_JUMP_TIME * MAX_JUMP_TIME);
                float bb = 4 * jumpHeight / MAX_JUMP_TIME;
                manager.height.height = (aa * jumpSex * jumpSex) + (bb * jumpSex); // y = ax^2 + bx


                //Check if done
                if (jumpSex > MAX_JUMP_TIME)
                {
                    //Leave
                    manager.height.height = 0;
                    Land();
                    return;
                }

                break;
            case 0:
                // increase down velocity up to maximum
                if (downVelocity < DOWN_MAX_VELOCITY)
                {
                    downVelocity += Time.deltaTime * DOWN_ACCELERATION;
                }

                // we move the rigidbody position to I guess account for any collision movement?
                manager.rigidBody.position += Vector2.down * downVelocity * Time.deltaTime;

                // Landing check
                if (manager.transform.position.y < BottomLedgeY)
                {
                    Land();
                    return;
                }
                break;
            // Jumping up (either 1 block or 2 blocks)
            case 1:
            case 2:
                // I added a scale here because 1x speed is too slow.
                jumpUpTimer += Time.deltaTime * UP_TIMESCALE;

                // Another system of equations. This is an equation for the midpoint of the jump to arrive at h+2, and end at h+1.
                // y = ax^2+bx+c

                // -2(h+3)         3h + 7
                // ------ * x^2 + -------- * x + starting Y position
                //   t^2             t
                float a = -(2 * (LedgeType + 3 + UP_LANDING_OFFSET)) / (UP_TOTAL_TIME * UP_TOTAL_TIME);
                float b = ((3 * (LedgeType + UP_LANDING_OFFSET)) + 7) / UP_TOTAL_TIME;
                manager.transform.position = new Vector3(manager.transform.position.x, a * jumpUpTimer * jumpUpTimer + b * jumpUpTimer + originalY, manager.transform.position.z);

                //Landing Check
                if (jumpUpTimer >= UP_TOTAL_TIME)
                {
                    Land();
                    return;
                }
                break;
        }

        void Land()
        {
            manager.stateTransitionTimer1 = 10; // 
            manager.SwitchState(new DefaultPlayerState());
        }
    }
}
