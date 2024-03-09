using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DirectionedObject), typeof(Grabbable))]
[System.Serializable]
public class Pushblock : MonoBehaviour
{
    Vector3 nextPosition;
    // Start is called before the first frame update
    void Start()
    {
        nextPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, Time.deltaTime*2);
        if(transform.position == nextPosition)
        {
            //Snap to grid
            transform.position += (Vector3)Vector2.one * 0.5f;
            transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
            transform.position -= (Vector3)Vector2.one * 0.5f;

            //just so we dont go into an infinite loop of going between our position and next position
            nextPosition = transform.position;
        }
    }
    
    public void Move(Vector2 direction) 
    {
        nextPosition = transform.position + (Vector3)direction;
    }
}
