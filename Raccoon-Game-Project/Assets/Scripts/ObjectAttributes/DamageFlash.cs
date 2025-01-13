using System.Collections;
using UnityEngine;

public class Flasher : MonoBehaviour
{
    [ColorUsage(true, true)]
    [SerializeField] Color flashColor = Color.white;
    [SerializeField] AnimationCurve flashCurve;
    [SerializeField] float flashTime = 0.25f;
    SpriteRenderer[] spriteRenderers;
    Material[] mats;
    Coroutine flashCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        mats = new Material[spriteRenderers.Length];
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            mats[i] = spriteRenderers[i].material;
        }
    }
    public void StartFlash()
    {
        StopAllCoroutines();
        flashCoroutine = StartCoroutine(DamageFlasher());
    }
    protected IEnumerator DamageFlasher()
    {
        foreach (var mat in mats)
        {
            mat.SetColor("_Flash", flashColor);
        }

        float currentFlashAmount = 0;
        float elapsedTime = 0;
        while (elapsedTime < flashTime)
        {
            foreach (var mat in mats)
            {
                mat.SetColor("_Flash", flashColor);
            }

            elapsedTime += Time.deltaTime;

            //lerp flash amount until 0
            currentFlashAmount = flashCurve.Evaluate(elapsedTime);
            // for each material child.
            foreach (var mat in mats)
            {
                mat.SetFloat("_Amount", currentFlashAmount);
            }
            yield return null;
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
