using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenObjEffect : MonoBehaviour
{
    void Update()
    {
        // Move over time to lock into centered grid
        transform.position = Vector3.MoveTowards(
            transform.position,
            SnapGrid.SnapToGridCentered(
                transform.position),
            Time.deltaTime);
    }
}
