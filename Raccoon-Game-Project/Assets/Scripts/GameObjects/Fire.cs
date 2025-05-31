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
    CollisionCheck findBurnable;
    // Start is called before the first frame update
    void Awake()
    {
        Invoke(nameof(PlayFireSound), UnityEngine.Random.Range(0, 0.5f));
        InvokeRepeating(nameof(FireTick), 4, 4); //start firing.
        fireInstance = Instantiate(fireEffect, transform).GetComponent<ParticleSystem>();


        //attach attachedObject.
        if (transform.parent != null && transform.parent.TryGetComponent(out Burnable burnable))
        {
            attachedObject = burnable;
            return;
        }
        //for some reason start is not being called here to initialize the find burnable.
        findBurnable = new(GetComponent<Collider2D>());
        findBurnable
        .SetType(CollisionCheck.CollisionType.SingleBox)
        .SetFindTriggers(true)
        .SetBoxSize(1.5f)
        .SetDebug(true);
        findBurnable.Evaluate<Burnable>((burnable) =>
        {
            attachedObject = burnable;
            transform.parent = attachedObject.transform;
            burnable.StartBurn();
        }, new((b) => b.GetComponentInChildren<Fire>() == null));
    }
    void Update()
    {
        if (attachedObject)
        {
            //slowly move towards our burnable object.
            transform.localPosition = Vector2.Lerp(transform.localPosition, Vector2.zero, Time.deltaTime / 2);
            transform.localScale = Vector2.Lerp(transform.localScale, Vector2.one, Time.deltaTime / 2);
        }
        if (Time.frameCount % 5 == 0)
        {
            if (FindObjectsByType<Fire>(FindObjectsSortMode.None).Length > 5)
            {
                foreach (var ps in GetComponentsInChildren<ParticleSystem>())
                {
                    var main = ps.main;
                    main.maxParticles = 3;
                }
            }
        }
    }

    void FireTick()
    {

        if (UnityEngine.Random.Range(0, 4) == 0) //Burn (1/4th chance)
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
        findBurnable = new(GetComponent<Collider2D>());
        findBurnable
        .SetType(CollisionCheck.CollisionType.SingleBox)
        .SetFindTriggers(true)
        .SetBoxSize(1.5f)
        .SetDebug(true);
        findBurnable.Evaluate<Burnable>((burnable) =>
        {
            GameObject newFire = Instantiate(replicator, burnable.transform);
            newFire.transform.position = transform.position;
        }, new((b) => b.GetComponentInChildren<Fire>() == null));
    }
    public void Die()
    {
        if (attachedObject)
        { attachedObject.Burn(); }
        transform.DetachChildren();
        fireInstance.Stop();
        Destroy(gameObject, 0.5f);
    }
}
