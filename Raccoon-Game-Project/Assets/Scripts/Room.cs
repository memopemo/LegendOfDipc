using UnityEngine;

public class Room : MonoBehaviour
{
    void Awake()
    {
        //find and verify entrance
        GameObject potentialEntrance = GameObject.Find("Entrance_" + SceneExitHandler.CurrentSceneExitIndex);
        if (!potentialEntrance) return;
        if (!potentialEntrance.TryGetComponent(out Entrance entrance)) return;

        // Tell important objects to set their directions.
        PlayerStateManager player = FindFirstObjectByType<PlayerStateManager>();
        Debug.Assert(player != null, "No Player Object to initialize in room");
        if(player)
        {
            player.RoomInitialize(entrance.transform.position, entrance.startingDirection);
        }
        
        CameraFocus cameraFocus = FindFirstObjectByType<CameraFocus>();
        Debug.Assert(cameraFocus != null, "No Camera to initialize in room");
        if(cameraFocus)
        {
            cameraFocus.InitializeCameraPosition((Vector2)entrance.startingDirection);
        }

    }
}
