using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MoveStraight>().direction = FindFirstObjectByType<PlayerStateManager>().directionedObject.direction * 15;
    }
}
