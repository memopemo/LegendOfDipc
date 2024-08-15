using UnityEngine;
using UnityEngine.SceneManagement;

//Exits the current scene and into a new scene's entrance.
public class Exit : MonoBehaviour
{
    [SerializeField] SceneReference scene;
    [SerializeField] int entranceNumber; // The place we want to exit out to
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsInvoking())
        {
            return;
        }
        if (collision.TryGetComponent(out PlayerStateManager _))
        {
            #if DEBUG
                Enter();
            #endif
            FindFirstObjectByType<CircleFadeInUI>().Out(); //play fadeout animation
            Invoke(nameof(Enter), 1); //exit scene and enter next scene.
        }
    }
    void Enter()
    {
        SceneExitHandler.CurrentSceneExitIndex = entranceNumber;
        SceneManager.LoadScene(scene);
    }
}

//These are to keep our next scene's data global to use it between scenes.
public static class SceneExitHandler
{
    public static int CurrentSceneExitIndex = 0;
}

public static class KeepUnderwaterPositionExitHandler
{
    public static Vector2 position = Vector2.zero;
}
