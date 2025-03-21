using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class WaterReflection : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    SpriteRenderer followRenderer;
    const float yOffset = 1;
    const float distanceBehindWater = 4;
    float tspFade;
    bool exiting;
    Heightable maybeHeightable;
    // Start is called before the first frame update
    void Start()
    {
        tspFade = 0;
        exiting = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.clear;
        followRenderer = transform.parent.GetComponent<SpriteRenderer>();
    }
    private void OnValidate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, distanceBehindWater);
    }

    // Update is called once per frame
    void Update()
    {
        if(followRenderer == null)
        {
            Destroy(gameObject);
        }

        // Set Fade
        tspFade += Time.deltaTime * (exiting ? -1: 1);
        tspFade = Mathf.Clamp(tspFade, 0,1);
        spriteRenderer.color = new Color(1,1,1,tspFade);
        if(exiting && tspFade <= 0)
        {
            Destroy(gameObject);
        }

        transform.position = followRenderer.transform.position;
        transform.position += Vector3.forward * distanceBehindWater;

        if (followRenderer.TryGetComponent(out Heightable h))
        {
            transform.position += Vector3.down * (h.height + yOffset);
        }
        else 
        {
            transform.position += Vector3.down * (followRenderer.transform.localPosition.y + yOffset);
        }

        spriteRenderer.sprite = followRenderer.sprite;
        spriteRenderer.flipX = followRenderer.flipX;
        spriteRenderer.flipY = !followRenderer.flipY;
    }
    public void Exit()
    {
        exiting = true;
    }
}
