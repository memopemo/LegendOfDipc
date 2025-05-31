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

    PlayerStateManager player;
    Platform currentPlatform;
    Collider2D box;


    // Start is called before the first frame update
    void Start()
    {
        height = GetComponent<Heightable>();
        TryGetComponent(out player);
        TryGetComponent(out box);

    }

    // Update is called once per frame
    void Update()
    {
        if (height && height.height > 0) 
        {
            if(currentPlatform != null)
            {
                currentPlatform = null;
                transform.parent = null;
            }
            return;
        }
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
            if (Time.frameCount % 3 != 0) return; //only run every 3 frames.
            CollisionCheck pitCheck = new(box);
            pitCheck
            .SetType(CollisionCheck.CollisionType.Point)
            .SetFindTriggers(true)
            .SetDebug(true).SetRelativePosition(box.offset);
            if(pitCheck.Evaluate<Platform>((t)=>{currentPlatform = t; transform.parent = t.transform;}))
            {
                return;
            }
            pitCheck.Evaluate<Pit>((p) =>
            {
                if (p.TryGetComponent(out Water _))
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
                else
                {
                    Fall();
                    StartFalling();
                }
            });
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
        foreach (Transform t in transform)
        {
            if (t.GetComponent<PlayerStateManager>())
            {
                t.parent = null;
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
        foreach (Transform t in transform)
        {
            if (t.GetComponent<PlayerStateManager>())
            {
                t.parent = null;
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
