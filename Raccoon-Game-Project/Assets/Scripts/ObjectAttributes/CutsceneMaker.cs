using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CutsceneMaker : MonoBehaviour
{
    [SerializeField] List<CutsceneAction> Actions;
    public void StartCutscene()
    {
        if (Actions == null || Actions.Count == 0) return;
        StartCoroutine(nameof(StartCutsceneCoroutine));
    }
    public IEnumerator StartCutsceneCoroutine()
    {
        print("Called");
        FreezeManager.FreezeAll<CutSceneFreezer>();
        foreach (var item in Actions)
        {
            item.Action.Invoke();
            yield return new WaitForSeconds(item.WaitTime);
        }
        FreezeManager.UnfreezeAll<CutSceneFreezer>();
        Actions.RemoveAll((item) => item.isTemporary);
    }
    public void AddTemporaryCutsceneAction(UnityAction action, float time)
    {
        CutsceneAction cutsceneAction = new(action, time);
        Actions.Add(cutsceneAction);
    }
    public void AddTemporaryCutsceneAction(float time)
    {
        CutsceneAction cutsceneAction = new(time);
        Actions.Add(cutsceneAction);
    }
}

[System.Serializable]
public class CutsceneAction
{
    public UnityEvent Action;
    public float WaitTime;
    public bool isTemporary;
    public CutsceneAction(UnityAction action, float waitTime)
    {
        Action = new UnityEvent();
        Action.AddListener(action);
        WaitTime = waitTime;
        isTemporary = true;
    }
    //blank waiting
    public CutsceneAction(float waitTime)
    {
        Action = new UnityEvent();
        WaitTime = waitTime;
        isTemporary = true;
    }
}
