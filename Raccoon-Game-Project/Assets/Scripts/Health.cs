using UnityEngine;
using UnityEngine.Events;
// Generic Health Class for keeping track of damage done.
public class Health : MonoBehaviour
{
    public int currentHealth;
    public UnityEvent OnDie; // for effects to display when healed.
    [SerializeField] UnityEvent OnTakeDamage; // for effects to display when damaged.
    [SerializeField] UnityEvent OnHeal; // for effects to display when 
    public GameObject HealParticle;

    // Update is called once per frame
    void Update()
    {
        if (currentHealth > 0) return;
        else Die();
    }
    public void TakeDamage(int amount)
    {

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
            return;
        }
        OnTakeDamage.Invoke();

    }
    public virtual void Heal(int amount)
    {
        currentHealth += amount;
        Instantiate(HealParticle, transform);
        OnHeal.Invoke();
    }
    public virtual void Die()
    {
        currentHealth = 0;
        OnDie.Invoke();
        gameObject.SetActive(false);
    }
    public void SetHeath(int health)
    {
        if (currentHealth <= 0)
        {
            Die();
            return;
        }
        currentHealth = health;
    }
}
