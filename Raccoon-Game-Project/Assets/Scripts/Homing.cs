using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//poopy

public class Homing : MonoBehaviour
{
    public Transform target;

    [Range(0,1)]
    public float slack; //how loose is the follow? 0 to 1

    public float speed;

    Vector2 laggedTargetPosition;

    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //lag target behind according to slack
        laggedTargetPosition = Vector2.Lerp(laggedTargetPosition, target.position, 1-slack);
        transform.position = Vector2.MoveTowards(transform.position, laggedTargetPosition, Time.deltaTime*speed);
    }
}
