using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DrainWarpPipe : MonoBehaviour
{
    bool endWalk;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<PlayerStateManager>())
        {
            FreezeManager.FreezeAll<CutSceneFreezer>();
            StartCoroutine(nameof(WalkIn));
            endWalk = false;
            Invoke(nameof(EndWalk), 1f);
        }
    }
    IEnumerator WalkIn()
    {
        PlayerStateManager player = FindFirstObjectByType<PlayerStateManager>();
        player.SetAnimation(25);
        while (!endWalk)
        {
            player.transform.position += Vector3.up * Time.deltaTime;
            yield return null;
        }
        UIPipeSelection pipeSelection = FindFirstObjectByType<UIPipeSelection>();
        pipeSelection.Show();

        yield return new WaitUntil(() => pipeSelection.holdDrainExit == false);

        //walk out
        endWalk = false;
        Invoke(nameof(EndWalk), 1f);
        while (!endWalk)
        {
            player.transform.position += Vector3.down * Time.deltaTime;
            yield return null;
        }
        FreezeManager.UnfreezeAll<CutSceneFreezer>();
        endWalk = false;
    }

    void EndWalk() => endWalk = true;
}
