using UnityEngine;

public static class Rotation
{
    public static float DirectionToAngle(Vector2 n)
    {
        return Vector2.SignedAngle(Vector2.up, n);
    }
}