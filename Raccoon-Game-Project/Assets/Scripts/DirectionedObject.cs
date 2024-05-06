using System;
using UnityEngine;
/// <summary>
/// This is for any object that has an up, down, and left/right direction.
/// </summary>
/// 
[RequireComponent(typeof(SpriteRenderer))]
public class DirectionedObject : MonoBehaviour
{
    public Vector2Int direction
    {
        get
        {
            if (Direction.x != 0 && Direction.y != 0)
            {
                return new Vector2Int(Direction.x, 0); //prioritize left/right over up down.
            }
            else return Direction;
        }
        set
        {
            Direction = value;
        }
    }
    Vector2Int Direction;
    [NonSerialized] SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        
        // This requires that every sprite asset with a direction must be right facing for their side.
        // I use < 0 instead of <= 0 because left would show for like a split second.
        spriteRenderer.flipX = Direction.x < 0;
    }
}
