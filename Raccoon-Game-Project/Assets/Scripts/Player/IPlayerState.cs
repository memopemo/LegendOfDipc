public interface IPlayerState
{
    /* ***IMPORTANT FUNCTIONALITY***
     * 
     * Any code run in OnUpdate will continue to run *even after* switching states. 
     * You must put control flow in your update function to return early if states need to switch instantaneously.
     * Such was the case of animation being set to 0 even after switching states and setting the animation to a new variable.
     */
    void OnUpdate(PlayerStateManager manager);
    void OnEnter(PlayerStateManager manager);
    void OnLeave(PlayerStateManager manager);
}
