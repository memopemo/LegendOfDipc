using System;
using UnityEngine;

public class WallShield : MonoBehaviour
{
    private const int WALL_KEY_ITEM_INDEX = 7;
    PlayerStateManager player;
    // Start is called before the first frame update
    void Start()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();

        //set direction based on player...
        player = FindFirstObjectByType<PlayerStateManager>();
        DirectionedObject directionedObject = GetComponent<DirectionedObject>();
        directionedObject.direction = player.directionedObject.direction;
        Vector2Int dir = directionedObject.direction;

        boxCollider.size = new Vector2((Mathf.Abs(dir.y) * 1.5f) + 0.5f, (MathF.Abs(dir.x) * 1.5f) + 0.5f); //makes the box either 2x0.5 or 0.5x2

        //check for if another wall shield is already active, if so, then destroy both of us (looks like we are just disabling the wall shield we just set up.)
        WallShield[] otherWallShields = FindObjectsByType<WallShield>(FindObjectsSortMode.None);
        foreach (WallShield otherWallShield in otherWallShields)
        {
            if (otherWallShield != this)
            {
                otherWallShield.Die(); //poof for them
                Destroy(gameObject); //no poof for us
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % 3 != 0) return;

        if (Vector2.Distance(player.transform.position, transform.position) > 15
            || SelectedItem.KeyItem != WALL_KEY_ITEM_INDEX)
        {
            Die();
        }
    }
    void Die()
    {
        GetComponent<PoofDestroy>().Poof();
        Destroy(gameObject);
    }
}
