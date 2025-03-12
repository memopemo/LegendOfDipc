using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FloorSound : MonoBehaviour
{
    NoiseMaker noiseMaker;
    public void Start()
    {
        noiseMaker = GetComponent<NoiseMaker>();
    }
    public void Play(Vector3 pos)
    {
        noiseMaker.Play(0, pos);
    }
}
