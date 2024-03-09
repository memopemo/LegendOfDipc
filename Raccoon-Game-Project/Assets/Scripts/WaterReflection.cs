using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class WaterReflection : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public SpriteRenderer followRenderer;
    [SerializeField] float yOffset;
    [SerializeField] float distanceBehindWater;
    Heightable maybeHeightable;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
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
}
