using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DirectionedObject))]
public class ProjectileCreator : MonoBehaviour
{
    DirectionedObject directioned;
    bool isDone;

    void Start()
    {
        directioned = GetComponent<DirectionedObject>();
        isDone = true;
        SetProjectilePositions();
    }
    void Update()
    {
        if(!isDone)
        {
            foreach (Transform projectile in transform)
            {
                if (projectile.gameObject.activeSelf)
                {
                    return;
                }
            }
            Retract(); //retract sets done to true.
        }
    }

    // callable. launches projectiles if we arent done.
    public void Launch()
    {
        if (!isDone)
        {
            return;
        }
        foreach (Transform projectile in transform)
        {
            projectile.gameObject.SetActive(true);
            Projectile p = projectile.GetComponentInChildren<Projectile>();
            if(p)
            {
                p.isHitting = false;
            }
        }
        isDone = false;

    }
    void Retract()
    {
        if (isDone)
        {
            return;
        }
        SetProjectilePositions();
        isDone = true;
    }
    void SetProjectilePositions()
    {
        foreach (Transform projectile in transform)
        {
            projectile.transform.position = transform.position + (Vector3)(Vector2)directioned.direction;
        }
    }
}
