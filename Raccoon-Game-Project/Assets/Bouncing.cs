using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bouncing : MonoBehaviour
{
    [SerializeField] AnimationCurve bounce;
    float time;
    void Update()
    {
        GetComponent<Heightable>().height = bounce.Evaluate(time);
        time += Time.deltaTime;
    }
    public bool IsBounceDone()
    {
        return time > bounce.keys.Last().time;
    }
}
