using UnityEngine;

public class DestroyFurniturePlayerState : IPlayerState
{
    public void OnEnter(PlayerStateManager manager)
    {

    }

    public void OnLeave(PlayerStateManager manager)
    {

    }

    public void OnUpdate(PlayerStateManager manager)
    {
        CommonPlayerState.UpdateDirection(manager);
        CommonPlayerState.MovePlayerRaw(manager, CommonPlayerState.DEFAULT_SPEED);
        manager.directionCheck.SetFindTriggers(true);
        manager.directionCheck.Evaluate<Interactable>((interact) => { interact.PlayerCanInteract(); });
        manager.directionCheck.SetFindTriggers(false);
        if (Buttons.IsButtonDown(Buttons.Sword))
        {

            manager.directionCheck.Evaluate<FurnitureObject>((furniture) =>
            {
                //GameObject.FindFirstObjectByType<HouseFurnitureCreator>().DestroyFurniture((Vector2)manager.transform.position + manager.directionedObject.direction);
                manager.SwitchState(new DefaultPlayerState());
            });
            manager.directionCheck.SetFindTriggers(true);
            manager.directionCheck.Evaluate<Interactable>((interact) => { interact.Interact(); });
            manager.directionCheck.SetFindTriggers(false);
        }


    }
}