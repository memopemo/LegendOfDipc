using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CutsceneMaker : MonoBehaviour
{
    [SerializeField] List<CutsceneAction> Actions;
    public void StartCutscene()
    {
        StartCoroutine(nameof(StartCutsceneCoroutine));
    }
    public IEnumerator StartCutsceneCoroutine()
    {
        FreezeManager.FreezeAll<CutSceneFreezer>();
        foreach (var item in Actions)
        {
            item.Action.Invoke();
            yield return new WaitForSeconds(item.WaitTime);
        }
        FreezeManager.UnfreezeAll<CutSceneFreezer>();
    }
}

[System.Serializable]
public class CutsceneAction
{
    public UnityEvent Action;
    public float WaitTime;
}
