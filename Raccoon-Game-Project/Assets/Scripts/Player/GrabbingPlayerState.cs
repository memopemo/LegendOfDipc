using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

// Push, pull, lift
public class GrabbingPlayerState : IPlayerState
{
    Grabbable grabbable;

    //how long does it take to activate a push/pull when holding the direction down?
    const float secsToPushPull = 1f;
    float secsPulling;
    float secsPushing;
    CollisionCheck pushPullCheck;
    const int GRAB_SFX = 9;
    const int PUSH_PULL_SFX = 10;

    public GrabbingPlayerState(Grabbable grabbable)
    {
        this.grabbable = grabbable;
    }

    public void OnEnter(PlayerStateManager manager)
    {
        if (grabbable.gameObject.scene != manager.gameObject.scene)
        {
            SceneManager.MoveGameObjectToScene(grabbable.gameObject, manager.gameObject.scene);
        }
        manager.transform.parent = grabbable.transform;

        //Snap to grid
        manager.transform.localPosition = -(Vector3)(Vector2)manager.directionedObject.direction;
        manager.transform.position = SnapGrid.SnapToGridCentered(manager.transform.position);
        pushPullCheck = new(grabbable.GetComponent<Collider2D>());
        pushPullCheck
            .SetType(CollisionCheck.CollisionType.DraggedBox)
            .SetBoxSize(0.90f)
            .SetDragPositionFromDirection(-1, manager.directionedObject)
            .SetDebug(true);
        manager.gameObject.AddComponent<ExclusionAttribute>();
        manager.noiseMaker.Play(GRAB_SFX);
    }

    public void OnLeave(PlayerStateManager manager)
    {
        manager.transform.parent = null;
        Object.Destroy(manager.GetComponent<ExclusionAttribute>());
    }

    public void OnUpdate(PlayerStateManager manager)
    {
        //Debug.Log(manager.transform.parent);
        if (manager.transform.parent == null || !Buttons.IsButtonHeld(Buttons.Sword))
        {
            manager.SwitchState(new DefaultPlayerState());
            grabbable.Ungrab();
            return;
        }
        //wanting to move opposite of direction (pulling)
        if (manager.rawInput == -manager.directionedObject.direction)
        {
            secsPushing = 0;
            if (secsPulling == 0)
            {
                manager.noiseMaker.Play(PUSH_PULL_SFX);
                grabbable.StartPull(-manager.directionedObject.direction);
            }
            secsPulling += Time.deltaTime;
            //Check if we can lift.
            if (grabbable.TryGetComponent(out Liftable l) && SaveManager.GetSave().ObtainedKeyUnselectableItems[1])
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
            if (secsPushing == 0)
            {
                manager.noiseMaker.Play(PUSH_PULL_SFX);
                grabbable.StartPush(manager.directionedObject.direction);
            }
            secsPushing += Time.deltaTime;
            manager.animator.SetAnimation(12);
        }
        else
        {
            if (secsPulling > 0)
            {
                grabbable.EndPull(-manager.directionedObject.direction);
            }
            if (secsPushing > 0)
            {
                grabbable.EndPush(manager.directionedObject.direction);
            }
            secsPulling = 0;
            secsPushing = 0;
            manager.animator.SetAnimation(11);
        }

        if (secsPulling > 1f)
        {
            //Check behind player if they can pull it and not risk squishing themself.
            pushPullCheck.SetDragPositionFromDirection(-2, manager.directionedObject);

            if (!pushPullCheck.EvaluateAnythingBut<ExclusionAttribute>((a) => { Debug.Log(a.gameObject); }))
            {
                Debug.Log("Pulled a " + grabbable.name);
                grabbable.Pull(-manager.directionedObject.direction);
                secsPulling = 0;
            }
            grabbable.gameObject.layer = 0;
        }
        if (secsPushing > 1f)
        {
            pushPullCheck.SetDragPositionFromDirection(1, manager.directionedObject);
            if (!pushPullCheck.EvaluateAnythingBut<ExclusionAttribute>((a) => { Debug.Log(a.gameObject); }))
            {
                Debug.Log("Pushed a " + grabbable.name);
                grabbable.Push(manager.directionedObject.direction);
                secsPushing = 0;
            };
            grabbable.gameObject.layer = 0;

        }
    }
}
