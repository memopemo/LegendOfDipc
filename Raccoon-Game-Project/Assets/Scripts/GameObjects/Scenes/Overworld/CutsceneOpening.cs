using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Cutscene : MonoBehaviour
{
    public void Activate()
    {
        FreezeManager.FreezeAll<CutSceneFreezer>();
        GetComponent<PlayableDirector>().Play();
    }
    public void Done()
    {
        FreezeManager.UnfreezeAll<CutSceneFreezer>();
    }

}
