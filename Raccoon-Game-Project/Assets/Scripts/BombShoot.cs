using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombShoot : HeldPlayerItem
{
    [SerializeField] int ammo;
    bool used;
    [SerializeField] GameObject shoot;

    new void Start()
    {
        base.Start();
        //players direction will stay the same
        transform.localPosition = new Vector2(0,0) + direction;
        InvokeRepeating(nameof(Shoot), 2, 1);
        GetComponent<DirectionedObject>().direction = direction;
    }
    protected override void Update()
    {
        if(!Input.GetButton("Fire2"))
        {
            if(used)
            {
                (player.currentPlayerState as HeldItemPlayerState).ExitUsed(player);
                Die();
            }
            else
            {
                (player.currentPlayerState as HeldItemPlayerState).ExitCanceled(player);
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
                player.DecrementConsumableItem();
                used = true;
            }

            ammo--;

            //offset bullet visually to better match where the rocket is.

            var obj = Instantiate(shoot, transform.position, transform.rotation);
            obj.GetComponent<DirectionedObject>().direction = direction;

            (player.currentPlayerState as HeldItemPlayerState).Kickback(player);

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
