public class DummyEnemy : Enemy
{
    public override void AIOnTakeDamage()
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
    public override void AIUpdate()
    {
    }
}
