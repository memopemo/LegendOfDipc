using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingDecal : MonoBehaviour
{
    float timeAlive;
    [SerializeField] float timeUntilFade;
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        timeAlive = timeUntilFade;
        Destroy(gameObject, timeUntilFade);
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive -= Time.deltaTime;
        sr.color = new Color(sr.color.r,sr.color.g,sr.color.b, Mathf.Clamp01(timeAlive*4/timeUntilFade)); //only fades in the last 1/4th
    }
}
