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
        Invoke(nameof(PlayFireSound), UnityEngine.Random.Range(0, 0.5f));
        InvokeRepeating(nameof(FireTick), 4,4); //start firing.
        fireInstance = Instantiate(fireEffect, transform).GetComponent<ParticleSystem>();
        

        //attach attachedObject.
        if(transform.parent != null && transform.parent.TryGetComponent(out Burnable burnable))
        {
            attachedObject = burnable;
            return;
        }
        Physics2D.queriesHitTriggers = true;
        List<RaycastHit2D> results = new();
        int _ = Physics2D.BoxCast(transform.position, Vector2.one*1.5f, 0, Vector2.zero, cf, results, 0);
        Physics2D.queriesHitTriggers = false;
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
        if(Time.frameCount % 5 == 0)
        {
            if(FindObjectsByType<Fire>(FindObjectsSortMode.None).Length > 5)
            {
                foreach(var ps in GetComponentsInChildren<ParticleSystem>())
                {
                    var main = ps.main;
                    main.maxParticles = 3;
                }   
            }
        }
    }

    void FireTick()
    {
        
        if(UnityEngine.Random.Range(0,4) == 0) //Burn (1/4th chance)
        {
            Die();
        }
        else //Spread (3/4ths chance)
        {
            SpreadFire();
        }
        Invoke(nameof(PlayFireSound), UnityEngine.Random.Range(0, 0.5f));
    }
    void PlayFireSound()
    {
        GetComponent<NoiseMaker>().Play(0);
    }

    void SpreadFire()
    {
        Physics2D.queriesHitTriggers = true;
        List<RaycastHit2D> results = new();
        int _ = Physics2D.BoxCast(transform.position, Vector2.one*1.5f, 0, Vector2.zero, cf, results, 0);
        Physics2D.queriesHitTriggers = false;
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
