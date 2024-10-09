using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGFloorRepeat : MonoBehaviour
{
    Vector3 initPos;
    float increment;
    void Start(){initPos = transform.localPosition;}
    // Update is called once per frame
    void Update()
    {
        increment += Time.deltaTime;
        transform.localPosition = initPos + Vector3.left * increment;
        if(increment >= 17)
        {
            increment = 0;
        }
    }
}
