using UnityEngine;

namespace Animator2D
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(DirectionedObject))]
    public class Animator2D : MonoBehaviour
    {
        SpriteRenderer spriteRenderer;
        DirectionedObject directionedObject;

        public AnimationData[] animations;
        public int currentAnimation;
        private int currentFrameTick; //NOTE: Relying on being 60fps. using faster fps is not accounted for!
        public int currentAnimationFrame;

        public void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            directionedObject = GetComponent<DirectionedObject>();
            spriteRenderer.sprite = GetFrameBasedOnDirection(currentAnimationFrame);
        }

        public void FixedUpdate()
        {
            if (currentFrameTick == animations[currentAnimation].SideAnimationFrames[currentAnimationFrame].HoldForTicks)
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
                if (currentAnimationFrame > animations[currentAnimation].SideAnimationFrames.Count - 1)
                {
                    currentAnimationFrame = animations[currentAnimation].LoopStart;
                }
            }
            if (currentAnimationFrame > animations[currentAnimation].SideAnimationFrames.Count - 1)
            {
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
            if (animations[animationIndex].SideAnimationFrames.Count < frame)
            {
                Debug.LogWarning("frame count: " + animationIndex + " is not in range of frames: " + animations[animationIndex].SideAnimationFrames.Count);
                return;
            }
            currentAnimation = animationIndex;
            currentAnimationFrame = frame;
            spriteRenderer.sprite = GetFrameBasedOnDirection(frame);
            currentFrameTick = 0;
        }
        public void RestartAnimation()
        {
            currentAnimationFrame = 0;
            spriteRenderer.sprite = GetFrameBasedOnDirection(0);
            currentFrameTick = 0;
        }

        Sprite GetFrameBasedOnDirection(int frame)
        {
            if (directionedObject.direction == Vector2Int.up)
            {
                return animations[currentAnimation].UpAnimationFrames[frame].Sprite;
            }
            else if (directionedObject.direction == Vector2Int.down)
            {
                return animations[currentAnimation].DownAnimationFrames[frame].Sprite;
            }
            else
            {
                //This covers even the 4 angled directions (1,1), (-1,-1), (-1,1), (1,-1)
                return animations[currentAnimation].SideAnimationFrames[frame].Sprite;
            }
        }
    }

    [System.Serializable]
    public class AnimFrame
    {
        public Sprite Sprite;
        public int HoldForTicks;
    }
}

