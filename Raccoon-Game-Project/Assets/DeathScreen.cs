using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    SpriteRenderer sr;
    const float SPEED = 1;
    float blackness;
    float blueness;
    IEnumerator Start()
    {
        DontDestroyOnLoad(gameObject);
        blackness = 0;
        blueness = 0;
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(1, 0, 0, 1);
        FreezeMmanager.FreezeAll<CutSceneFreezer>();

        yield return new WaitForSeconds(0.5f); //reaction to getting hit

        //fade clear to red
        while (blueness < 1)
        {
            blackness += Time.deltaTime * SPEED;
            blueness += Time.deltaTime * SPEED;
            sr.color = new Color(1, 0, blueness) * (1 - blackness);
            sr.color += Color.black;
            yield return null;
        }

        //load scene
        yield return SceneManager.LoadSceneAsync("SampleScene");
        PlayerHealth.UnDie();

        Destroy(gameObject);
    }
}
