using UnityEngine;

public class PlacementSecret : SecretPredicate
{
    Collider2D area;
    GameObject lookFor;
    public void Start()
    {
        area = GetComponent<Collider2D>();
    }
    public override string Evaluate()
    {
        Collider2D[] colliders = new Collider2D[1];
        area.Overlap(new ContactFilter2D(), colliders);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject == lookFor)
            {
                return "";
            }
        }
        return "None of the colliders overlapping this gameobject (if any) matched the Game Object we were looking for.";
    }
}
