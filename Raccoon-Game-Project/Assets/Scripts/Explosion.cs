using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] int amount;
    void Start()
    {
        FindAnyObjectByType<CameraFocus>().ShakeScreen(2);
        GetComponent<NoiseMaker>().Play(0);
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.TryGetComponent(out Health health))
        {
            //apply damage.
            health.TakeDamage(amount);
            //if enemy, apply knockback aswell.
            if(collider.TryGetComponent(out Enemy enemy))
            {
                enemy.ApplyKnockBack(transform);
            }
        }
    }
}
