using UnityEngine;
public class BoomerangPlayerState : IPlayerState
{

    const int THROW_ANIM = 2; //replace
    public void OnEnter(PlayerStateManager manager)
    {
        Object.Instantiate(manager.boomerang, manager.transform.position + (Vector3)(Vector2)manager.directionedObject.direction, Quaternion.identity);
        manager.stateTransitionTimer1 = 10;
        manager.animator.SetAnimation(1);
    }

    public void OnLeave(PlayerStateManager manager)
    {
    }

    public void OnUpdate(PlayerStateManager manager)
    {
        if(manager.stateTransitionTimer1 == 1)
        {
            
        }
        if(manager.stateTransitionTimer1 >= 10) return;
        manager.SwitchState(new DefaultPlayerState());
    }
}