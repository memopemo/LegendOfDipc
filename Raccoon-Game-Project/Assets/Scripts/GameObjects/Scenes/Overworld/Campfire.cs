using System.Collections;
using Animator2D;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Campfire : MonoBehaviour
{
    DayNightSystem.TimesOfDay setTimeOfDay;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Fire fire))
        {
            fire.Die();
            Light();
        }
    }
    public void Light()
    {
        GetComponent<SimpleAnimator2D>().SetAnimation(1);
        GetComponent<Light2D>().enabled = true;
        GetComponent<Interactable>().enabled = true;
    }
    void DeLight()
    {
        GetComponent<SimpleAnimator2D>().SetAnimation(0);
        GetComponent<Light2D>().enabled = false;
        GetComponent<Interactable>().enabled = false;
    }
    public void OnInteract()
    {
        Sleep(DayNightSystem.TimesOfDay.DAWN);
    }
    public void Sleep(DayNightSystem.TimesOfDay timeOfDay)
    {
        setTimeOfDay = timeOfDay;
        StartCoroutine(nameof(SleepCutscene));
    }
    IEnumerator SleepCutscene()
    {
        FreezeManager.FreezeAll<CutSceneFreezer>();
        yield return new WaitForSeconds(3);
        DayNightSystem.SetTime(setTimeOfDay);
        DeLight();
        FreezeManager.UnfreezeAll<CutSceneFreezer>();
    }
}
