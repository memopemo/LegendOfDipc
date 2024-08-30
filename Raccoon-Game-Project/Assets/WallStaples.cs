using UnityEngine;

public class WallStaples : MonoBehaviour
{

    PlayerStateManager playerStateManager;
    // Start is called before the first frame update
    void Start()
    {
        playerStateManager = FindAnyObjectByType<PlayerStateManager>();
        if(playerStateManager && playerStateManager.directionedObject.direction.y != 0) 
        {
            (playerStateManager.currentPlayerState as DefaultPlayerState).CheckWallStapleWall(playerStateManager);
        }
        Destroy(gameObject);
        
    }
}
