using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaryPitch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
