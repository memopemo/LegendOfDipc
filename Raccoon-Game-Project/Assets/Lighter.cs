using System.Collections;
using System.Collections.Generic;
using Animator2D;
using UnityEngine;

public class Lighter : MonoBehaviour
{
    void Start()
    {
        Invoke(nameof(CreateFire),0.5f);
    }
    void CreateFire()
    {
        GetComponent<PoofDestroy>().Poof();
        Destroy(gameObject);
    }

}
