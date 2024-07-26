using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StatusEffectArea : MonoBehaviour
{
    [SerializeField] Status.Effect effect;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Tick()
    {
        List<Collider2D> colliders = new();
        GetComponent<Collider2D>().Overlap(colliders);
        foreach (Collider2D collider in colliders)
        {
            TrySetStatus(collider);
        }
    }

    
    void OnTriggerEnter2D(Collider2D collider)
    {
        TrySetStatus(collider);
    }
    void TrySetStatus(Collider2D collider)
    {
        if(collider.TryGetComponent(out Status status))
        {
            status.SetStatus(effect);
        }
    }
}
