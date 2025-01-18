using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//Exits the current scene and into a new scene's entrance.
public class Exit : MonoBehaviour
{
    [SerializeField] SceneReference scene;
    [SerializeField] int entranceNumber; // The place we want to exit out to
    enum ExitDirection { Up, Down, Left, Right };
    [SerializeField] ExitDirection exitDirection;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsInvoking())
        {
            return;
        }
        if (collision.TryGetComponent(out PlayerStateManager _))
        {
            FindFirstObjectByType<CircleFadeInUI>().Out(); //play fadeout animation
            FreezeManager.FreezeAll<PauseFreezer>();
            StartCoroutine(nameof(WalkPlayerOut));
            try
            {
                FindAnyObjectByType<SongPlayer>()?.OnSceneChange(scene);
            }
            catch
            {
                Debug.Log("No Song Player.");
            }

            Invoke(nameof(Enter), 1); //exit scene and enter next scene.
        }
    }
    IEnumerator WalkPlayerOut()
    {
        PlayerStateManager player = FindFirstObjectByType<PlayerStateManager>();
        player.SetAnimation(25);
        //huh, ive never seen this. this is really cool!
        var exitDirectionVector = exitDirection switch
        {
            ExitDirection.Up => Vector3.up,
            ExitDirection.Down => Vector3.down,
            ExitDirection.Left => Vector3.left,
            ExitDirection.Right => Vector3.right,
            _ => Vector3.zero,
        };
        while (true)
        {
            player.transform.position += exitDirectionVector * Time.deltaTime;
            yield return null;
        }
    }
    void Enter()
    {
        if (Random.value == 0)
        {
            SceneManager.LoadScene("error");
        }
        else
        {
            SceneManager.LoadScene(scene);
        }

        ExitHandler.ExitChangingRooms(entranceNumber);
    }
}

//These are to keep our next scene's data global to use it between scenes.
public static class ExitHandler
{
    public static Vector2 position = Vector2.zero;
    public static int exitObjectIndex = 0;
    public enum Type { ChangingRooms, LoadingSavePoint, GoingUnderwater, Surfacing, ViaPipe }
    public static Type type;
    public static void ExitChangingRooms(int toEntranceIndex)
    {
        type = Type.ChangingRooms;
        exitObjectIndex = toEntranceIndex;
    }
    public static void ExitLoadingSavePoint()
    {
        type = Type.LoadingSavePoint;
        exitObjectIndex = SaveManager.GetSave().SavePoint;
    }
    public static void ExitGoingUnderwater(Vector2 positionInSurfaceScene)
    {
        type = Type.GoingUnderwater;
        position = positionInSurfaceScene;
    }
    public static void ExitSurfacing(Vector2 positionInUnderwaterScene)
    {
        type = Type.Surfacing;
        position = positionInUnderwaterScene;
    }
    public static void ExitViaPipe(int pipeDestinationIndex)
    {
        type = Type.ViaPipe;
        exitObjectIndex = pipeDestinationIndex;
    }
}
