using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Orbit : MonoBehaviour
{
    public float orbitSpeed;

    void Update()
    {
        transform.Rotate(0,0,orbitSpeed*360*Time.deltaTime);
    }

}
