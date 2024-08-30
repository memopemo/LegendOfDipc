using UnityEngine;
//Player Health referenced throughout the life of the game, in between rooms and areas that may not even have any player in them.
public static class PlayerHealth
{
    public static int STARTING_HEART_CONTAINERS = 3;
    public static int currentHealth = 6;
    #if DEBUG
    public static bool debugLockHealth = false;
    #endif

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
        player.GetComponent<Flasher>().StartFlash();
        GameObject.FindFirstObjectByType<CameraFocus>().ShakeScreen(1);
    }
    public static void Heal(int amount, PlayerStateManager player = null)
    {
        currentHealth += amount;
        if(player)
        {
            GameObject.Instantiate(player.healParticle, player.transform);
        }
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
