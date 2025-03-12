using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsHighlighted : MonoBehaviour
{
    [SerializeField] GameObject highlightedSprite;
    [SerializeField] Color findColor;
    GameObject currentlyLoadedSprite;
    void Update()
    {
        if (DemonBuffs.HasBuff(DemonBuffs.DemonBuff.Vision) && !currentlyLoadedSprite)
        {
            currentlyLoadedSprite = Instantiate(highlightedSprite, transform);
            currentlyLoadedSprite.GetComponent<Renderer>().material.SetColor("_SampleColor1", findColor);
        }
        else if (!DemonBuffs.HasBuff(DemonBuffs.DemonBuff.Vision) && currentlyLoadedSprite)
        {
            Destroy(currentlyLoadedSprite);
        }
    }
}
