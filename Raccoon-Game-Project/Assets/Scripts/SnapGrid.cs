using UnityEngine;
public static class SnapGrid
{
    public static Vector3 SnapToGridCentered3D(Vector3 v)
    {
        v += (Vector3)Vector2.one * 0.5f;
        v = new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
        v -= (Vector3)Vector2.one * 0.5f;
        return v;
    }
    public static Vector2 SnapToGridCentered2D(Vector2 v)
    {
        v += Vector2.one * 0.5f;
        v = new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
        v -= Vector2.one * 0.5f;
        return v;
    }
}