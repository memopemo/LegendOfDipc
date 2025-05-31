using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    SpriteRenderer sr;
    const float SPEED = 1;
    float blackness;
    float blueness;
    [SerializeField] SceneReference overworldScene;
    IEnumerator Start()
    {
        DontDestroyOnLoad(gameObject);
        FindFirstObjectByType<Canvas>().gameObject.SetActive(false);
        FindFirstObjectByType<SongPlayer>()?.StartFadeOut(0.01f);
        blackness = 0;
        blueness = 0;
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(1, 0, 0, 1);
        FreezeManager.FreezeAll<CutSceneFreezer>();
        
        yield return new WaitForSeconds(0.5f); //reaction to getting hit

        //fade clear to red
        while (blueness < 1)
        {
            blackness += Time.deltaTime * SPEED;
            blueness += Time.deltaTime * SPEED;
            sr.color = new Color(1, 0, blueness) * (1 - blackness);
            sr.color += Color.black;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
            yield return null;
        }

        //load scene

        ExitHandler.ExitLoadingSavePoint();
        SceneManager.LoadScene(overworldScene); //loadSceneAsync() causes the circle fade in to not work
        
        //subscribe to whent the active scene actually does change.
        SceneManager.activeSceneChanged += UnDie;

    }
    void UnDie(Scene x, Scene y)
    {
        PlayerHealth.UnDie();
        SceneManager.activeSceneChanged -= UnDie;
        FindAnyObjectByType<SongPlayer>()?.Restart();
        Destroy(gameObject);

    }
}
