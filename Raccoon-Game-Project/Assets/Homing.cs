using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : MonoBehaviour
{
    public Transform target;
    FineDirectionedObject fineDirectioned;
    public float slack; //how loose is the follow?
    
    // Start is called before the first frame update
    void Start()
    {
        fineDirectioned = GetComponent<FineDirectionedObject>();
    }

    // Update is called once per frame
    void Update()
    {
        fineDirectioned.direction = Vector3.RotateTowards(fineDirectioned.direction, target.position - transform.position, 2*Mathf.PI-slack*Time.deltaTime, 10-slack*Time.deltaTime);
    }
}
