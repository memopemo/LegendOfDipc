using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    Burnable attachedObject; //object to burn.
    [SerializeField] GameObject replicator; //new fire to make.
    [SerializeField] GameObject fireEffect; 
    ParticleSystem fireInstance;
    ContactFilter2D cf = new()
    {
        useTriggers = true,
    };
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<NoiseMaker>().Play(0);
        print(gameObject.name);
        InvokeRepeating(nameof(FireTick), 4,4); //start firing.
        fireInstance = Instantiate(fireEffect, transform).GetComponent<ParticleSystem>();
        

        //attach attachedObject.
        if(transform.parent != null && transform.parent.TryGetComponent(out Burnable burnable))
        {
            attachedObject = burnable;
            return;
        }
        
        List<RaycastHit2D> results = new();
        int _ = Physics2D.BoxCast(transform.position, Vector2.one*1.5f, 0, Vector2.zero, cf, results, 0);
        foreach (RaycastHit2D hit in results)
        {
            if (hit.collider.TryGetComponent(out Burnable burnable1))
            {
                Fire alreadyHasFire = burnable1.transform.GetComponentInChildren<Fire>();
                if (alreadyHasFire) continue;
                attachedObject = burnable1;
                transform.parent = attachedObject.transform;
                return;
            }
        }   
    }
    void Update()
    {
        if(attachedObject)
        {
            //slowly move towards our burnable object.
            transform.localPosition = Vector2.Lerp(transform.localPosition, Vector2.zero, Time.deltaTime/2);
            transform.localScale = Vector2.Lerp(transform.localScale, Vector2.one, Time.deltaTime/2);
        }
    }

    void FireTick()
    {
        GetComponent<NoiseMaker>().Play(0);
        if(UnityEngine.Random.Range(0,4) == 0) //Burn (1/4th chance)
        {
            Die();
        }
        else //Spread (3/4ths chance)
        {
            SpreadFire();
        }
    }

    void SpreadFire()
    {
        List<RaycastHit2D> results = new();
        int _ = Physics2D.BoxCast(transform.position, Vector2.one*1.5f, 0, Vector2.zero, cf, results, 0);
        foreach (RaycastHit2D hit in results)
        {
            if (hit.collider.TryGetComponent(out Burnable burnable))
            {
                Fire alreadyHasFire = burnable.transform.GetComponentInChildren<Fire>();
                if (alreadyHasFire) continue;
                GameObject go = Instantiate(replicator, burnable.transform);
                go.transform.position = transform.position;
                return;
            }
        }
    }
    void Die()
    {
            if(attachedObject)
            { attachedObject.Burn(); }
            transform.DetachChildren();
            fireInstance.Stop();
            Destroy(gameObject,0.5f);
    }
}
