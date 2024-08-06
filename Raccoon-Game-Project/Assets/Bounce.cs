using System;
using UnityEngine;

[RequireComponent(typeof(FineDirectionedObject))]
public class Bounce : MonoBehaviour
{
    FineDirectionedObject fineDirectionedObject;
    Projectile projectile;
    [SerializeField] int bounces;
    void Start()
    {
        fineDirectionedObject = GetComponent<FineDirectionedObject>();
        projectile = GetComponent<Projectile>();
    }

    //  Reflect projectile with respect to the wall and direction.
    //  wallDirection is essentially the "normal" of the wall. (with regard to 2D space instead of 3D)
    //  how do i get the wall direction?
    public void BounceProjectile(Vector2 wallDirection)
    {
        bounces--;
        if(bounces <= -1)
        {
            projectile.Die();
            return;
        }
        Invoke(nameof(ClearHitting), 0.01f);
        fineDirectionedObject.direction = Vector2.Reflect(fineDirectionedObject.direction, wallDirection);
        

    }
    void ClearHitting()
    {
        projectile.isHitting = false;
    }
}
