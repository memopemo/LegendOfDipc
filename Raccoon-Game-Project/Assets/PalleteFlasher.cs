using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalleteFlasher : MonoBehaviour
{
    [SerializeField] Color find;
    [SerializeField] Color[] flashingPattern;
    byte animationIndex;
    [SerializeField] float rate;
    bool active;
    Material[] mats;
    SpriteRenderer[] spriteRenderers;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(OnUpdateColors), rate, rate);
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        mats = new Material[spriteRenderers.Length];
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            mats[i] = spriteRenderers[i].material;
        }
        foreach (var material in mats)
        {
            material.SetColor("_SampleColor1", find);
            material.SetColor("_ReplaceColor1", find);
        }

    }
    public void StartFlash()
    {
        animationIndex = 0;
        active = true;
    }
    void OnUpdateColors()
    {
        if (active && animationIndex < flashingPattern.Length)
        {
            foreach (var material in mats)
            {
                material.SetColor("_ReplaceColor1", flashingPattern[animationIndex]);
            }
            animationIndex++;
        }
        else
        {
            active = false;
            animationIndex = 0;
        }
    }
}
