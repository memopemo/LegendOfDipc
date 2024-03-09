using UnityEngine;

public class Room : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //find and verify entrance
        GameObject potentialEntrance = GameObject.Find("Entrance_" + SceneExitHandler.CurrentSceneExitIndex);
        if (!potentialEntrance) return;
        if (!potentialEntrance.TryGetComponent(out Entrance entrance)) return;

        // Tell important objects to set their directions.
        FindFirstObjectByType<PlayerStateManager>().RoomInitialize(entrance.transform.position, entrance.startingDirection);
        FindFirstObjectByType<CameraFocus>().InitializeCameraPosition((Vector2)entrance.startingDirection);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
