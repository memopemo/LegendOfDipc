using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopySprite : MonoBehaviour
{
    SpriteRenderer baseSprite;
    SpriteRenderer decoySprite;
    // Start is called before the first frame update
    void Start()
    {
        baseSprite = transform.parent.GetComponent<SpriteRenderer>();
        decoySprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        decoySprite.material = baseSprite.material;
        decoySprite.sprite = baseSprite.sprite;
        decoySprite.flipX = baseSprite.flipX;
        decoySprite.flipY = baseSprite.flipY;
        decoySprite.color = baseSprite.color;
    }
}
