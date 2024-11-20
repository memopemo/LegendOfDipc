using UnityEngine;

public class Stairs : MonoBehaviour
{

    //Horizontal Stairs move the player up at the same time the player moves left/right.
    //Vertical Stairs slow the player down.
    enum Orientation { Horizontal, Vertical }
    [SerializeField] Orientation orientation;
}
