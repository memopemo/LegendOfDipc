
using System;
using UnityEngine;

namespace Animator2D
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SimpleAnimator2D : MonoBehaviour
    {
        public SimpleAnimationData[] animations;
        SpriteRenderer spriteRenderer;
        [NonSerialized] public int currentAnimation;
        [NonSerialized] private int currentFrameTick; //NOTE: Relying on being 60fps. using faster fps is not accounted for!
        [NonSerialized] public int currentAnimationFrame;
        [NonSerialized] public int finishedFrames;

        public void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = GetFrameBasedOnDirection(currentAnimationFrame);
        }

        public void FixedUpdate()
        {
            if (currentFrameTick == animations[currentAnimation].AnimationFrames[currentAnimationFrame].HoldForTicks)
            {
                GetNextFrame();
            }
            else
            {
                currentFrameTick++;
            }
        }

        public void GetNextFrame()
        {
            currentAnimationFrame += 1;
            //Loop or keep the animation frame on its final frame.
            if (animations[currentAnimation].Looping)
            {
                if (currentAnimationFrame > animations[currentAnimation].AnimationFrames.Count - 1)
                {
                    currentAnimationFrame = animations[currentAnimation].LoopStart;
                }
            }
            if (currentAnimationFrame > animations[currentAnimation].AnimationFrames.Count - 1)
            {
                finishedFrames += 1;
                currentAnimationFrame -= 1;
            }

            currentFrameTick = 0;

            spriteRenderer.sprite = GetFrameBasedOnDirection(currentAnimationFrame);
        }

        public void SetAnimation(int animationIndex, int frame = 0)
        {
            if (animationIndex == currentAnimation) return;
            if (animations.GetUpperBound(0) < animationIndex)
            {
                Debug.LogWarning("index: " + animationIndex + " is not in range of animations: " + animations.GetUpperBound(0));
                return;
            }
            if (animations[animationIndex].AnimationFrames.Count < frame)
            {
                Debug.LogWarning("frame count: " + animationIndex + " is not in range of frames: " + animations[animationIndex].AnimationFrames.Count);
                return;
            }
            currentAnimation = animationIndex;
            currentAnimationFrame = frame;
            spriteRenderer.sprite = GetFrameBasedOnDirection(frame);
            currentFrameTick = 0;
            finishedFrames = 0;
        }
        public void RestartAnimation(int frame = 0)
        {
            currentAnimationFrame = frame;
            spriteRenderer.sprite = GetFrameBasedOnDirection(frame);
            currentFrameTick = 0;
        }
        Sprite GetFrameBasedOnDirection(int frame)
        {
            return animations[currentAnimation].AnimationFrames[frame].Sprite;
        }
    }
}