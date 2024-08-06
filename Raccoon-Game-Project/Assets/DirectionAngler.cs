using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FineDirectionedObject))]
public class DirectionAngler : MonoBehaviour
{
    FineDirectionedObject fineDirectionedObject;
    // Start is called before the first frame update
    void Start()
    {
        fineDirectionedObject = GetComponent<FineDirectionedObject>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(0,0,Rotation.DirectionToAngle(fineDirectionedObject.direction));
    }
}
