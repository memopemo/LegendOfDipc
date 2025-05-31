using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    DamagesPlayer damagesPlayer;
    void Start()
    {
        FindAnyObjectByType<CameraFocus>().ShakeScreen(2);
        GetComponent<NoiseMaker>().Play(0);
        damagesPlayer = GetComponent<DamagesPlayer>();
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.TryGetComponent(out PlayerStateManager player))
        {
           player.TakeDamage(damagesPlayer);
        }
    }
}
