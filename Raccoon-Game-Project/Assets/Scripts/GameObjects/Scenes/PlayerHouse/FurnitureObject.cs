using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Pushblock), typeof(BoxCollider2D))]

public class FurnitureObject : MonoBehaviour
{
    public Furniture furniture;
    public void OnMove(Vector2 direction)
    {
        GetComponent<Pushblock>().Move(direction);
        Vector2Int localPos = Vector2Int.RoundToInt((Vector2)transform.localPosition); //essentially 4.00000000011265 -> 4
        Vector2Int newPos = Vector2Int.RoundToInt((Vector2)transform.localPosition + direction);
        int index = localPos.x + (16 * -localPos.y);
        int newIndex = newPos.x + (16 * -newPos.y);
        FindFirstObjectByType<FurnitureManager>().MoveFurniture(index, newIndex, furniture);
    }
}
