public class HurtPlayerState : IPlayerState
{
    public void OnEnter(PlayerStateManager manager)
    {

    }

    public void OnLeave(PlayerStateManager manager)
    {

    }

    public void OnUpdate(PlayerStateManager manager)
    {
        CommonPlayerState.CheckWater(manager);
        if (manager.stateTransitionTimer1 == 0)
        {
            manager.SwitchState(new DefaultPlayerState());
        }
    }
}