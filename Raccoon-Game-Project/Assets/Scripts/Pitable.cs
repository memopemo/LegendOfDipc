using System.Collections.Generic;
using UnityEngine;

public class Pitable : MonoBehaviour
{
    [SerializeField] GameObject splashParticle;
    [SerializeField] GameObject fallingEffect;
    Heightable height;
    float timePitted;
    const float TIME_FALLING_BEFORE_DONE = 2f;
    bool FallingDownPit; // False is Drowning.
    ContactFilter2D contactFilter = new ContactFilter2D()
    {
        useTriggers = true,
    };

    PlayerStateManager player;


    // Start is called before the first frame update
    void Start()
    {
        height = GetComponent<Heightable>();
        TryGetComponent(out player);

    }

    // Update is called once per frame
    void Update()
    {
        if (height.height > 0) return;
        if (timePitted > 0)
        {
            if (timePitted > TIME_FALLING_BEFORE_DONE)
            {
                OnDone();
            }
            else
            {
                timePitted += Time.deltaTime;
            }
            return;
        }
        else
        {
            //Check for pit/water we cant go down.
            List<Collider2D> results = new List<Collider2D>();
            _ = Physics2D.OverlapPoint(transform.position, contactFilter, results);
            foreach (Collider2D c in results)
            {
                if (c)
                {
                    if (c.TryGetComponent(out Water _))
                    {
                        if (player)
                        {
                            if (SaveManager.GetSave().ObtainedKeyUnselectableItems[2])
                            {
                                return;
                            }
                        }
                        Sink();
                        StartFalling();

                    }
                    if(c.TryGetComponent(out Pit _))
                    {
                        Fall();
                        StartFalling();
                    }
                }
            }
        }
    }
    void StartFalling()
    {
        if (player)
        {
            player.SwitchState(new FallPlayerState());
        }
        else
        {
            OnDone();
            return;
        }
        timePitted += Time.deltaTime;
    }
    void Fall()
    {
        Instantiate(fallingEffect, transform.position, Quaternion.identity);
        //Unparent player if he is attached to it.
        foreach (Transform t in transform)
        {
            if (player)
            {
               transform.parent = null;
            }
        }
    }
    void Sink()
    {
        Instantiate(splashParticle, transform.position, Quaternion.identity);
        //Unparent player if he is attached to it.
        foreach (Transform t in transform)
        {
            if (player)
            {
               transform.parent = null;
            }
        }

    }
    void OnDone()
    {
        timePitted = 0;
        if (player)
        {
            player.SetPlayerBack();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
