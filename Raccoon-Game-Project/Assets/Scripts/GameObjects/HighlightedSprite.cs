using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightedSprite : MonoBehaviour
{
    SpriteRenderer sr;
    SpriteRenderer parentSR;
    Heightable heightable;
    // Start is called before the first frame update
    IEnumerator Start()
    {

        sr = GetComponent<SpriteRenderer>();
        parentSR = transform.parent.GetComponent<SpriteRenderer>();
        if (transform.parent.TryGetComponent(out Heightable a))
        {
            yield return new WaitUntil(() => a.didStart);
            transform.SetParent(a.decoySprite.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        sr.enabled = transform.parent.GetComponent<SpriteRenderer>().enabled;
        sr.sprite = parentSR.sprite;
        sr.flipX = parentSR.flipX;
        sr.flipY = parentSR.flipY;

    }
}
