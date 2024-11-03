using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Heightable))]
public class Pitable : MonoBehaviour
{
    [SerializeField] GameObject splashParticle;
    [SerializeField] GameObject fallingEffect;
    Heightable height;
    float timePitted;
    const float TIME_FALLING_BEFORE_DONE = 2f;
    bool FallingDownPit; // False is Drowning.

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
            if (Time.frameCount % 3 != 0) return; //only run every 3 frames.
            CollisionCheck pitCheck = new(GetComponent<Collider2D>());
            pitCheck
            .SetType(CollisionCheck.CollisionType.Point)
            .SetFindTriggers(true)
            .SetDebug(true);
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
