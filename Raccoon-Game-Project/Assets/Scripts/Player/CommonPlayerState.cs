
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

class CommonPlayerState
{
    public const float DEFAULT_SPEED = 6;
    public const int BOOT_INVENTORY_INDEX = 4;
    public const int JUMP_HEIGHT_NORMAL = 1;
    public const int JUMP_HEIGHT_SUPER = 2;
    public static void MovePlayerRaw(PlayerStateManager manager, float speed)
    {
        manager.rigidBody.linearVelocity = manager.rawInput * (speed + manager.additionalSpeed);
    }
    public static void MovePlayerSmooth(PlayerStateManager manager, float speed)
    {
        manager.rigidBody.AddForce(manager.rawInput * (speed + manager.additionalSpeed) * Time.deltaTime * 60);
    }

    public static void UpdateDirection(PlayerStateManager manager)
    {
        //Debug.Log(manager.directionedObject.direction);
        //TODO: maybe add anti-stick-flick, doesnt seem to be a problem rn with normal gameplay, 
        // only when intentionally flicking does it occur.
        // or add an option to movement when stick flicking.
        if (manager.rawInput != Vector2.zero)
        {
            manager.directionedObject.direction = Vector2Int.RoundToInt(manager.rawInput.normalized);
        }

    }
    public static float GetJumpHeight()
    {

        //TODO: change jump height based on what we have. 
        if (SaveManager.GetSave().ObtainedKeyUnselectableItems[BOOT_INVENTORY_INDEX + 1])
        {
            return JUMP_HEIGHT_SUPER; //+1 so it looks bigger.
        }
        else if (SaveManager.GetSave().ObtainedKeyUnselectableItems[BOOT_INVENTORY_INDEX])
        {
            return JUMP_HEIGHT_NORMAL; //+1 so it looks bigger.
        }
        return 0;
        //else return JUMP_HEIGHT_NORMAL;
    }
}