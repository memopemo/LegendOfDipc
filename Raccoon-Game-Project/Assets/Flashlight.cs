using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : HeldPlayerItem
{
    new void Start()
    {
        base.Start();
        transform.localRotation = Quaternion.Euler(0,0, Rotation.DirectionToAngle(direction));
    }
}
