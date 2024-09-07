using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//loopable audio.
[CreateAssetMenu()]
public class Song : ScriptableObject
{
    public AudioClip song;
    public int LoopStartSamples; //how many samples until we enter the loop?
    public int LoopEndSamples; //how many samples until we loop?
}
