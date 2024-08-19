using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// Push, pull, lift
public class GrabbingPlayerState : IPlayerState
{
    Grabbable grabbable;

    //how long does it take to activate a push/pull when holding the direction down?
    const float secsToPushPull = 1f;
    float secsPulling;
    float secsPushing;

    public GrabbingPlayerState(Grabbable grabbable) 
    {  
        this.grabbable = grabbable;
    }

    public void OnEnter(PlayerStateManager manager)
    {
        manager.transform.parent = grabbable.transform;

        //Snap to grid
        manager.transform.localPosition = -(Vector3)(Vector2)manager.directionedObject.direction;
        manager.transform.position = SnapGrid.SnapToGridCentered(manager.transform.position);
    }

    public void OnLeave(PlayerStateManager manager)
    {
        manager.transform.parent = null;
    }

    public void OnUpdate(PlayerStateManager manager)
    {
        //Debug.Log(manager.transform.parent);
        if (manager.transform.parent == null)
        {
            manager.SwitchState(new DefaultPlayerState());
            return;
        }
        if (!Input.GetButton("Fire1"))
        {
            manager.SwitchState(new DefaultPlayerState());
            return;
        }
        //wanting to move opposite of direction (pulling)
        if (manager.rawInput == -manager.directionedObject.direction)
        {
            secsPushing = 0;
            secsPulling += Time.deltaTime;
            //Check if we can lift.
            if (grabbable.TryGetComponent(out Liftable l))
            {
                if (secsPulling > 0.5f)
                {
                    manager.stateTransitionTimer1 = 20;
                    manager.SwitchState(new LiftPlayerState(l)); 
                }
            }
            manager.animator.SetAnimation(13);
        }
        
        //wanting to move in direction (pushing)
        else if (manager.rawInput == manager.directionedObject.direction)
        {
            secsPulling = 0;
            secsPushing += Time.deltaTime;
            manager.animator.SetAnimation(12);
        }
        else
        {
            secsPulling = 0;
            secsPushing = 0;
            manager.animator.SetAnimation(11);
        }

        if(secsPulling > 1f) 
        {
            //Check behind player if they can pull it and not risk squishing themself.
            if (CommonPlayerState.ColliderAt(manager, -manager.directionedObject.direction, out _, grabbable.gameObject)) return;
            Debug.Log("Pulled a " + grabbable.name);
            grabbable.Pull(-manager.directionedObject.direction);
            secsPulling = 0;
        }
        if (secsPushing > 1f) 
        {
            //check 2 tiles infront of player if they can push it.
            //I guess this makes a requirement that all pushable objects must be 1x1
            if (CommonPlayerState.ColliderAt(manager, manager.directionedObject.direction * 2, out GameObject a, grabbable.gameObject)) 
            {
                Debug.Log(a);
                return;
            };
            Debug.Log("Pushed a " + grabbable.name);
            grabbable.Push(manager.directionedObject.direction);
            secsPushing = 0;
        }
    }
}
