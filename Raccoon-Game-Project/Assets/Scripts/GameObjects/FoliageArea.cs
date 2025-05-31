using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GenericSloper))]
public class FoliageArea : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] spriteRenderers;
    Color[] initialColors;
    GenericSloper genericSloper;
    // Start is called before the first frame update
    void Start()
    {
        genericSloper = GetComponent<GenericSloper>();
        initialColors = new Color[spriteRenderers.Length];
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            initialColors[i] = spriteRenderers[i].color;
            spriteRenderers[i].color = Color.clear;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].color = Color.Lerp(initialColors[i], Color.clear, genericSloper.value);
        }

    }
}
