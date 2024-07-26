using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffDisplay : MonoBehaviour
{
    [SerializeField] int buffToDisplay; //buff slot 1, 2, or 3?
    [SerializeField] Sprite[] graphics;
    Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        image.sprite = graphics[(int)DemonBuffs.demonBuffs[buffToDisplay]];
    }
}
