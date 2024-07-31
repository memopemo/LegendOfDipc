using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HeldPlayerItem))]
public class Flashlight : MonoBehaviour
{
    void Start()
    {
        HeldPlayerItem heldPlayerItem = GetComponent<HeldPlayerItem>();
        transform.localRotation = Quaternion.Euler(0,0, Rotation.DirectionToAngle(heldPlayerItem.direction));
    }
}
