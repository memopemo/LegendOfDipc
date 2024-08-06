using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(FineDirectionedObject))]

public class MoveStraight : MonoBehaviour
{
    FineDirectionedObject fineDirectionedObject;
    void Start()
    {
        fineDirectionedObject = GetComponent<FineDirectionedObject>();
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)fineDirectionedObject.direction * Time.deltaTime;
    }
}
