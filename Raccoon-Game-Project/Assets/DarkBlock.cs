using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DarkBlock : MonoBehaviour
{
    int strength;
    float timeBeingShinedOn;
    bool isBeingShinedOn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerStay2D(Collider2D collider){
        if(collider.TryGetComponent(out FlashlightLight light))
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, light.transform.position);
            if(hit)
            {
                print(hit.collider.name);
            }
        }
    }
}
