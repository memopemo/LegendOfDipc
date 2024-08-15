using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(HeldPlayerItem))]
public class BombShoot : MonoBehaviour
{
    [SerializeField] int ammo;
    bool used;
    [SerializeField] GameObject shoot;
    HeldPlayerItem heldPlayerItem;

    void Start()
    {
        heldPlayerItem = GetComponent<HeldPlayerItem>();
        //players direction will stay the same
        transform.localPosition = Vector2.zero + heldPlayerItem.player.directionedObject.direction;
        InvokeRepeating(nameof(Shoot), 2, 1);
    }
    void Update()
    {
        if(!Input.GetButton("Fire2"))
        {
            if(used)
            {
                //create the used particle
                (heldPlayerItem.player.currentPlayerState as HeldItemPlayerState).ExitUsed(heldPlayerItem.player);
                Die();
            }
            else
            {
                (heldPlayerItem.player.currentPlayerState as HeldItemPlayerState).ExitCanceled(heldPlayerItem.player);
                Destroy(gameObject);
            }
        }
    }

    void Shoot()
    {
        if(ammo > 0)
        {
            if(!used)
            {
                heldPlayerItem.player.DecrementConsumableItem();
                used = true;
            }

            ammo--;

            var obj = Instantiate(shoot, transform.position, transform.rotation);
            obj.GetComponent<DirectionedObject>().direction = heldPlayerItem.player.directionedObject.direction;

            (heldPlayerItem.player.currentPlayerState as HeldItemPlayerState).Kickback(heldPlayerItem.player);

            if(ammo == 0)
            {
                Invoke(nameof(Die), 0.5f);
            }
            Animator2D.Animator2D anim = GetComponent<Animator2D.Animator2D>();
            anim.SetAnimation(1);
            anim.RestartAnimation();
        }

    }
    void Die()
    {
        Destroy(gameObject);
        GetComponent<PoofDestroy>()?.Poof();
    }
}
