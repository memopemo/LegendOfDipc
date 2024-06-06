using UnityEngine;

/* These are for snapping a position to the nearest *center point* of the grid, rather than the actual grid coordinate.
 * This is essentially snapping to a *tile*, rather than a coordinate.
 */
public static class SnapGrid
{
    public static Vector3 SnapToGridCentered(Vector3 v)
    {
        v += (Vector3)Vector2.one * 0.5f; //add a half unit
        v = new Vector2(Mathf.Round(v.x), Mathf.Round(v.y)); //round it to nearest whole int
        v -= (Vector3)Vector2.one * 0.5f; //subtract half from rounded pos
        //the proof as to how this works is left as an excersise to the reader.
        return v;
    }
    public static Vector2 SnapToGridCentered(Vector2 v)
    {
        v += Vector2.one * 0.5f;
        v = new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
        v -= Vector2.one * 0.5f;
        return v;
    }
}