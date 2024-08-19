//Fuck Unity.
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//uh oh. if i rename this every sound effect will be erased.
public class ListWrapper
{
    public List<AudioClip> pool;
    public bool isCancelable;
    public bool varyPitch;
    public bool looping;
    public bool followNoiseMaker;
}