using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandProjectile : MonoBehaviour
{
    private const int PJ_SPEED = 15;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<FineDirectionedObject>().direction = FindFirstObjectByType<PlayerStateManager>().directionedObject.direction * PJ_SPEED;
    }
}
