using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

// Freezes this object's behaviopurs based on what it will freeze on
// Enabling freezing is strictly explicit. You must add this component if you want to freeze, and you must also subscribe to what you want to be frozen by.
// This system allows some objects to be frozen while others not.
class Freezer : MonoBehaviour
{
    List<Behaviour> behavioursFrozen = new();
    
    public void FreezeChildrenAndSelf()
    {
        //dont freeze if this behaviour is turned off.
        if(!enabled) return;
        //NOTE: only goes 1 child layer deep. I dont think i'll make this recursive.
        // But later on i may have to... fuck...
        Freeze(gameObject);
        foreach (Transform child in transform)
        {
            Freeze(child.gameObject);
        }
    }

    void Freeze(GameObject gob)
    {
        //Get everything that can be disabled.
        foreach (Behaviour behaviour in gob.GetComponents(typeof(Behaviour)).Cast<Behaviour>())
        {
            // add behaviours as necessary.
            if( !behaviour.enabled || behaviour is Freezer || behaviour is Camera || behaviour is AudioListener || behaviour is PixelPerfectCamera)   
            {
                continue;
            }
            behaviour.enabled = false;
            behavioursFrozen.Add(behaviour);
        }  
        //stop rigidbody.
        if(TryGetComponent(out Rigidbody2D rb) && gob == gameObject)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
    public void Unfreeze()
    {
        foreach (var frozenBehaviour in behavioursFrozen)
        {
            frozenBehaviour.enabled = true;
        }
        behavioursFrozen.Clear();
        if(TryGetComponent(out Rigidbody2D rb))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
