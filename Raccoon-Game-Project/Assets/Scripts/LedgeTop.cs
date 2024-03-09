using System;
using UnityEngine;

public class LedgeTop : MonoBehaviour
{
    [NonSerialized] public Collider2D collide;
    public LedgeBottom bottom;
    [Range(0, 3)]
    public int layersToDecrease;
    private void Start()
    {
        collide = GetComponent<Collider2D>();
    }
}
