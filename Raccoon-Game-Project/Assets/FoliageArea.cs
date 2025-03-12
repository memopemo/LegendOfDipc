using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoliageArea : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] spriteRenderers;
    Color[] initialColors;
    float sec;
    float targetSec;
    public bool inverse;
    // Start is called before the first frame update
    void Start()
    {
        targetSec = inverse ? 0 : 1;
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
            spriteRenderers[i].color = Color.Lerp(initialColors[i], Color.clear, sec);
        }
        if (sec > targetSec) sec -= Time.deltaTime;
        if (sec < targetSec) sec += Time.deltaTime;
    }
    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.GetComponent<PlayerStateManager>())
        {
            targetSec = inverse ? 1 : 0;
        }
    }
    void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.GetComponent<PlayerStateManager>())
        {
            targetSec = inverse ? 0 : 1;
        }
    }
}
