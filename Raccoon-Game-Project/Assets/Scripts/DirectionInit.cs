using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionInit : MonoBehaviour
{
    [SerializeField] Vector2Int direction;
    void Start()
    {
        GetComponent<DirectionedObject>().direction = direction;
        Destroy(this);
    }
}
