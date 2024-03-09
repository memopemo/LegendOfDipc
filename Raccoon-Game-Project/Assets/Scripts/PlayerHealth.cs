public class PlayerHealth : Health
{
    //Do not destroy player. Instead, find gamecontroller to start death cutscene.

    public override void Die()
    {
        currentHealth = 0;
        OnDie.Invoke();
    }
}
