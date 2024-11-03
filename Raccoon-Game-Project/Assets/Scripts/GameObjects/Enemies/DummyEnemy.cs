using UnityEngine;

public class DummyEnemy : MonoBehaviour
{
    float velocity;
    Heightable heightable;
    float jumpTimer;
    public void Start()
    {
        heightable = GetComponent<Heightable>();
    }
    public void OnTakeDamage()
    {

        Animator2D.Animator2D animator2D = GetComponent<Animator2D.Animator2D>();
        if (animator2D.currentAnimation == 1)
        {
            animator2D.RestartAnimation();
        }
        else
        {
            animator2D.SetAnimation(1);
        }

    }
    public void Jump()
    {
        velocity = -0.25f;
        transform.localScale = new(0.8f, 1.2f, 1);
        jumpTimer = 1;

    }
    public void Update()
    {
        Timer.DecrementTimer(ref jumpTimer);
        if (jumpTimer <= 0)
        {
            Jump();
        }

        //add gravity
        velocity += Time.deltaTime * 1f;

        //apply to height.
        heightable.height -= velocity;
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * 5);


        if (heightable.height <= 0)
        {
            heightable.height = 0;
            velocity = 0;
            transform.localScale = new(1.2f, 0.8f, 1);
        }
    }
}
