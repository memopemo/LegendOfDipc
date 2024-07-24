using Animator2D;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleAnim2D", menuName = "SimpleAnimationData", order = 2)]
public class SimpleAnimationData : ScriptableObject
{
    public List<AnimFrame> AnimationFrames;
    public bool Looping;
    public int LoopStart;
    public int LoopEnd;

}