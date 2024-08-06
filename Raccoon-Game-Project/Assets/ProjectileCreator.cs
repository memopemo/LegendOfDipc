using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DirectionedObject))]
public class ProjectileCreator : MonoBehaviour
{
    DirectionedObject directioned;
    bool isDone;
    List<Projectile> projectiles;

    void Start()
    {
        directioned = GetComponent<DirectionedObject>();
        isDone = true;
        projectiles = new(GetComponentsInChildren<Projectile>());
        SetProjectilePositions();
    }
    void Update()
    {
        if(!isDone)
        {
            foreach (var projectile in projectiles)
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
        projectiles.ForEach(x => {x.gameObject.SetActive(true); x.isHitting = false ;});
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
        projectiles.ForEach(x=> x.transform.position = transform.position + (Vector3)(Vector2)directioned.direction);
    }
}
