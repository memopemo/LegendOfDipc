using UnityEngine;
//Player Health referenced throughout the life of the game, in between rooms and areas that may not even have any player in them.
public static class PlayerHealth
{
    public static int currentHealth = 8;
    public static bool debugLockHealth = false;
    //Do not destroy player. Instead, find gamecontroller to start death cutscene.

    public static void Die()
    {
        currentHealth = 0;
    }
    public static void TakeDamage(int amount, PlayerStateManager player)
    {

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
            return;
        }
        player.GetComponent<DamageFlash>().StartFlash();
        GameObject.FindFirstObjectByType<CameraFocus>().ShakeScreen(1);
    }
    public static void Heal(int amount, PlayerStateManager player)
    {
        currentHealth += amount;
        GameObject.Instantiate(player.healParticle, player.transform);
    }
    public static void SetHealth(int health)
    {
        if (currentHealth <= 0)
        {
            Die();
            return;
        }
        currentHealth = health;
    }
}
