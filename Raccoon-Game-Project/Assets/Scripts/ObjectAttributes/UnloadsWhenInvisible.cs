using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnloadsWhenInvisible : MonoBehaviour
{
    CameraFocus cf;
    // Start is called before the first frame update
    void Start()
    {
        cf = FindFirstObjectByType<CameraFocus>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!cf.IsOnScreen(transform.position))
        {
            Destroy(gameObject);
        }
    }
}
