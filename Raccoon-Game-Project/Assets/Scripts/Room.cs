using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Room : MonoBehaviour
{
    void Start()
    {
        // Tell important objects to set their directions.
        PlayerStateManager player = FindFirstObjectByType<PlayerStateManager>();
        Debug.Assert(player != null, "No Player Object to initialize in room");

        CameraFocus cameraFocus = FindFirstObjectByType<CameraFocus>();
        Debug.Assert(cameraFocus != null, "No Camera to initialize in room");

        if (!player || !cameraFocus) return;
        switch (ExitHandler.type)
        {
            case ExitHandler.Type.ChangingRooms:
                RegularEntrance(player, cameraFocus, ExitHandler.exitObjectIndex);
                return;
            case ExitHandler.Type.LoadingSavePoint:
                if (ExitHandler.exitObjectIndex == 0)
                {
                    //initial save position (no jumping out of bin)
                    //start overworld cutscene
                    cameraFocus.InitializeCameraPosition(Vector2.zero);
                    FindFirstObjectByType<Cutscene>().Activate();
                    return;
                }
                foreach (SavePoint savePoint in FindObjectsByType<SavePoint>(FindObjectsSortMode.None))
                {

                    if (savePoint.SavePointID == ExitHandler.exitObjectIndex)
                    {
                        player.DisableSprite();
                        player.SetInitState(new NoInputPlayerState());
                        savePoint.SpitOutPlayerReloadGame(player);
                        cameraFocus.InitializeCameraPosition(Vector2.zero);
                        return; //this has to be a return for some reason else Regular Entrance gets called.
                    }
                }
                Debug.LogWarning($"No Save Point found with index {ExitHandler.exitObjectIndex}, loading entrance 0");
                RegularEntrance(player, cameraFocus, 0);
                return;
            case ExitHandler.Type.GoingUnderwater:
                player.SetInitState(new UnderwaterPlayerState());
                player.transform.position = ExitHandler.position;
                cameraFocus.InitializeCameraPosition(Vector2.zero);
                return;
            case ExitHandler.Type.Surfacing:
                //we allow the default state to enter splash state immediately.
                player.transform.position = ExitHandler.position;
                cameraFocus.InitializeCameraPosition(Vector2.zero);
                return;
            case ExitHandler.Type.ViaPipe:
                GameObject potentialPipe = GameObject.Find("DrainPipe_" + ExitHandler.exitObjectIndex);
                if (potentialPipe && potentialPipe.GetComponentInChildren<DrainWarpPipe>())
                {
                    player.RoomInitialize(potentialPipe.transform.position + Vector3.down * 2, Vector2Int.down);
                    cameraFocus.InitializeCameraPosition(Vector2.down);
                    return;
                }
                Debug.LogWarning($"No Pipe found with index {ExitHandler.exitObjectIndex}, loading entrance 0");
                RegularEntrance(player, cameraFocus, 0);
                return;
        }
    }
    void RegularEntrance(PlayerStateManager player, CameraFocus cameraFocus, int entranceIndex)
    {
        GameObject potentialEntrance = GameObject.Find("Entrance_" + entranceIndex);
        if (potentialEntrance && potentialEntrance.TryGetComponent(out Entrance entrance))
        {
            player.RoomInitialize(entrance.transform.position, entrance.startingDirection);
            cameraFocus.InitializeCameraPosition((Vector2)entrance.startingDirection);
        }

        else FallbackEntrance(player, cameraFocus);

    }

    //for when we have really fucked up
    void FallbackEntrance(PlayerStateManager player, CameraFocus cameraFocus)
    {
        Debug.LogWarning($"No Entrance found too!? Just leave the player where he's at.");
        cameraFocus.InitializeCameraPosition(Vector3.down);
    }
}
