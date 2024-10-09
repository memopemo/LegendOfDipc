using System.Collections;
using UnityEngine;

public class Saxophone : MonoBehaviour
{
    private IEnumerator Start()
    {
        PlayerStateManager player = FindFirstObjectByType<PlayerStateManager>();
        player.SwitchState(new NoInputPlayerState());

        CameraFocus camera = FindFirstObjectByType<CameraFocus>();

        FreezeMmanager.FreezeAll<CutSceneFreezer>();
        //animate player
        //play sax sound
        yield return new WaitForSeconds(3f);

        FreezeMmanager.UnfreezeAll<CutSceneFreezer>();

        Saxable[] saxables = FindObjectsByType<Saxable>(FindObjectsSortMode.None);

        foreach (var saxable in saxables)
        {
            if (camera.IsOnScreen(saxable.transform.position))
            {
                saxable.OnSaxed.Invoke();
            }
            yield return null; //dont calculate all at once.
        }

        player.SwitchState(new DefaultPlayerState());

        Destroy(gameObject);

    }
}
