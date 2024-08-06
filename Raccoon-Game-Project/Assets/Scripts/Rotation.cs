using UnityEngine;

public static class Rotation
{
    public static float DirectionToAngle(Vector2 n)
    {
        return Vector2.SignedAngle(Vector2.up, n);
    }

    // Turn our hit vector2 into an angle on the z axis.
    // This is only used for visual particle effects.
    public static Quaternion Get2DAngleFromPoints(Vector2 v1, Vector2 v2)
    {
        Vector2 direction = (v1 - v2).normalized;

        // Math stuff I copied online. I added negatives to fix wrong direction issues.
        float angle = Mathf.Atan2(direction.y, -direction.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle, -Vector3.forward);
    }
}