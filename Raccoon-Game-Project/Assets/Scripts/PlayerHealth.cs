using System.Linq;
using UnityEngine;
//Player Health referenced throughout the life of the game, in between rooms and areas that may not even have any player in them.
public static class PlayerHealth
{
    public static int STARTING_HEART_CONTAINERS = 3;
    public static int currentHealth = STARTING_HEART_CONTAINERS * 2;
    public static bool dying;
#if DEBUG
    public static bool debugLockHealth = false;
#endif

    //Do not destroy player.

    public static void Die(PlayerStateManager player)
    {
        currentHealth = 0;
        dying = true;
        player.Die();

    }
    public static void TakeDamage(int amount, PlayerStateManager player)
    {
        if (dying) return;
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die(player);
            return;
        }
        GameObject.FindFirstObjectByType<CameraFocus>().ShakeScreen(1);
    }
    public static void Heal(int amount, PlayerStateManager player = null)
    {
        currentHealth += amount;
        Mathf.Clamp(currentHealth, 0, (SaveManager.GetSave().HeartContainersCollected.Count((x) => x) + 3) * 2);
        if (player)
        {
            GameObject.Instantiate(player.healParticle, player.transform);
        }
    }
    public static int GetMaxHealth() => (SaveManager.GetSave().HeartContainersCollected.Count((x) => x) + 3) * 2;

    public static void SetHealth(int health)
    {
        currentHealth = health;
    }
    public static void UnDie()
    {
        currentHealth = STARTING_HEART_CONTAINERS * 2;
        dying = false;


    }
}
