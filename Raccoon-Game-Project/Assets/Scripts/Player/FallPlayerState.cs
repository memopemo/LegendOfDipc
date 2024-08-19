public class FallPlayerState : IPlayerState
{
    float SecsFalling;
    float SetbackAfter;
    void IPlayerState.OnEnter(PlayerStateManager manager)
    {
        manager.DisableSprite();
    }

    void IPlayerState.OnLeave(PlayerStateManager manager)
    {
        manager.EnableSprite();
    }

    void IPlayerState.OnUpdate(PlayerStateManager manager)
    {
    }
}