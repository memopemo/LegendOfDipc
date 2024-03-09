using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Go up and down until reaching the end.
public class ClimbingPlayerState : IPlayerState
{
    float bottomY;
    float topY;
    const float CLIMB_SPEED = 4;

    public ClimbingPlayerState(float bottom, float top) 
    {
        bottomY = bottom;
        topY = top;
    }
    public void OnEnter(PlayerStateManager manager)
    {
        manager.GetComponent<Collider2D>().isTrigger = true;

        //nudge player up/down depending on direction.
        if(manager.directionedObject.direction.y == -1) 
        {
            manager.transform.position += Vector3.down * 0.5f;
        }
        else if (manager.directionedObject.direction.y == 1)
        {
            manager.transform.position += Vector3.up * 0.5f;
        }
        
    }

    public void OnLeave(PlayerStateManager manager)
    {
        manager.GetComponent<Collider2D>().isTrigger = false;
    }

    public void OnUpdate(PlayerStateManager manager)
    {
        
        manager.rigidBody.velocityY = manager.inputY * CLIMB_SPEED + manager.additionalSpeed;
        CommonPlayerState.UpdateDirection(manager); //Player can look left/right (bc it looks cute :)

        if (manager.transform.position.y > topY+0.5f)
        {
            manager.stateTransitionTimer1 = 10;
            manager.directionedObject.direction = Vector2Int.up;
            manager.SwitchState(new DefaultPlayerState());
        }

        if (manager.transform.position.y < bottomY) 
        {
            manager.stateTransitionTimer2 = 10;
            manager.directionedObject.direction = Vector2Int.up;
            manager.SwitchState(new DefaultPlayerState());
        }

        // Move Player Over Time to lock into grid
        manager.transform.position = Vector3.MoveTowards(
            manager.transform.position,
            new Vector3(Common.SnapToGrid(
                manager.transform.position).x,
                manager.transform.position.y),
            Time.deltaTime);

        if (manager.rigidBody.velocityY != 0) 
        {
            manager.animator.SetAnimation(9);
        }
        else
        {
            manager.animator.SetAnimation(10);
        }

    }
}
