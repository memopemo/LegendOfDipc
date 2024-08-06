using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnetizable : MonoBehaviour
{
    DirectionedObject directionedObject;
    [NonSerialized] public Rigidbody2D rb;

    void Start()
    {
        directionedObject = GetComponent<DirectionedObject>();
    }
    public void BeAttracted(Vector2Int magnetDirection)
    {
        directionedObject.direction = -magnetDirection;
    }
}
