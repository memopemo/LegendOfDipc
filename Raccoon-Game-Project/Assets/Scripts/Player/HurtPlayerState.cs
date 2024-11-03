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
        manager.waterCheck.Evaluate<Water>((_) => manager.SwitchState(new SwimPlayerState()));
        if (manager.stateTransitionTimer1 == 0)
        {
            manager.SwitchState(new DefaultPlayerState());
        }
    }
}