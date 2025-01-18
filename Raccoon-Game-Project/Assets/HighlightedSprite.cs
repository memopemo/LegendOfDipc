using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightedSprite : MonoBehaviour
{
    SpriteRenderer sr;
    SpriteRenderer parentSR;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        parentSR = transform.parent.GetComponent<SpriteRenderer>();
        if (transform.parent.TryGetComponent(out Heightable a))
        {
            transform.SetParent(a.decoySprite.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        sr.sprite = parentSR.sprite;
        sr.flipX = parentSR.flipX;
        sr.flipY = parentSR.flipY;

    }
}
