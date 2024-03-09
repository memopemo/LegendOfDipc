using UnityEngine;
using UnityEngine.SceneManagement;

//Exits the current scene and into a new scene's entrance.
public class Exit : MonoBehaviour
{
    [SerializeField] SceneReference scene;
    [SerializeField] int entranceNumber; // The place we want to exit out to
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsInvoking())
        {
            return;
        }
        if (collision.TryGetComponent(out PlayerStateManager _))
        {
            FindFirstObjectByType<CircleFadeInUI>().Out();
            Invoke(nameof(Enter), 1);
        }
    }
    void Enter()
    {
        SceneExitHandler.CurrentSceneExitIndex = entranceNumber;
        SceneManager.LoadScene(scene);
    }
}
public static class SceneExitHandler
{
    public static int CurrentSceneExitIndex = 0;

}

public static class KeepPositionExitHandler
{
    public static Vector2 position = Vector2.zero;
}
