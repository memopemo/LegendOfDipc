using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Orbiting Projectile
[RequireComponent(typeof(FineDirectionedObject))]
public class Orbit : MonoBehaviour
{
    [SerializeField] Vector2 orbitSize;
    [SerializeField] float orbitSpeed;
    [SerializeField] Vector2 orbitOffset; //offsets the sin and cos values. (for diagonal orbits)
    float timeAlive;
    FineDirectionedObject fineDirectionedObject;
    void Start()
    {
        fineDirectionedObject = GetComponent<FineDirectionedObject>();
    }

    void Update()
    {
        Vector2 previousPosition = transform.localPosition;
        //update time by timescale.
        timeAlive += Time.deltaTime * orbitSpeed;

        //Calculate X and Y positions.
        float x = Mathf.Sin(timeAlive + orbitOffset.x);
        x *= orbitSize.x; //factor size of oribt on the x axis.
        x *= Mathf.Clamp(timeAlive, 0, 1); //orbit grows by time of being alive.

        float y = Mathf.Cos(timeAlive + orbitOffset.y);
        y *= orbitSize.y;
        y *= Mathf.Clamp(timeAlive, 0, 1); //orbit grows by time of being alive.

        Vector2 newPosition = new(x,y); 
        //set position based on calculated x and y, and offset it by our phantom orbit point 
        transform.localPosition = newPosition;
        
        fineDirectionedObject.direction = newPosition - previousPosition; //compare previous world position to new world position for direction.
        //yes, this cuts short the orbit and is not fully tangential to the orbit, but its such a miniscule difference and i dont care.
    }
}
