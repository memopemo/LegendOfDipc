using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ColorPalleteHandler : MonoBehaviour
{
    [SerializeField] Color[] color1ReplaceAnimation;
    [SerializeField] Color[] color2ReplaceAnimation;
    [SerializeField] Color[] color3ReplaceAnimation;
    [SerializeField] Color[] color4ReplaceAnimation;
    int animationIndex = 0;
    [SerializeField] float rate;
    Material material;
    void Start()
    {
        material = GetComponent<Renderer>().material;
        InvokeRepeating(nameof(OnUpdateColors), rate, rate);
    }
    void OnUpdateColors()
    {
        animationIndex++;
        SetColors(1, color1ReplaceAnimation);
        SetColors(2, color2ReplaceAnimation);
        SetColors(3, color3ReplaceAnimation);
        SetColors(4, color4ReplaceAnimation);
        void SetColors(int replaceIndex, Color[] colorAnimation)
        {
            material.SetColor("_ReplaceColor" + replaceIndex, colorAnimation[animationIndex % colorAnimation.Length]);
        }
    }

}
