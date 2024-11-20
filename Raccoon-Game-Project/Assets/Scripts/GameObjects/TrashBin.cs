using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Makes the player hop in and do something afterward, then waits to spit back out.
// Just for refilling health and saving. 
public abstract class TrashBin : MonoBehaviour
{
    Animator2D.Animator2D anim;
    PlayerStateManager player;

    private void Start()
    {
        anim = GetComponent<Animator2D.Animator2D>();
        player = FindAnyObjectByType<PlayerStateManager>();
        GetComponentInChildren<Flasher>().transform.position += Vector3.left * 0.1f;
        GetComponentInChildren<Flasher>().transform.position -= Vector3.left * 0.1f;
    }
    //hide actual player and show bin enter animation.
    public void OnPlayerUse()
    {
        TakeInPlayer();
    }
    private void TakeInPlayer()
    {
        player.SwitchState(new NoInputPlayerState());
        player.DisableSprite();
        anim.SetAnimation(1);
        Invoke(nameof(OnEnterDone), 1.75f);
    }
    public void SpitOutPlayerReloadGame(PlayerStateManager player)
    {
        player.directionedObject.direction = Vector2Int.right;
        player.transform.position = transform.position + new Vector3(1.435f, -1.622f, 0);
        Invoke(nameof(SpitOutPlayer), 0.3f);
    }
    public void SpitOutPlayer()
    {
        player.directionedObject.direction = Vector2Int.right;
        player.rigidBody.position = transform.position + new Vector3(1.435f, -1.622f, 0);
        anim.SetAnimation(2);
        Invoke(nameof(OnExitDone), 0.9f);
        GetComponentInChildren<Interactable>().enabled = true;
    }
    void OnExitDone()
    {
        player.SwitchState(new DefaultPlayerState());
        player.EnableSprite();

        anim.SetAnimation(0);

    }
    void OnEnterDone()
    {

        print("Done Entering");
        OnAction();
    }
    public abstract void OnAction();

}

