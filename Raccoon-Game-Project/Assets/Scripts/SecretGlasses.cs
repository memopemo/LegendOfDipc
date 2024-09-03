using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SecretGlasses : MonoBehaviour
{
    Camera cameraa;
    void Start()
    {
        cameraa = FindFirstObjectByType<Camera>();
        transform.parent = cameraa.transform;
        transform.localPosition = new Vector3(0, 0, 8); //put in front of camera.
        cameraa.GetComponent<UniversalAdditionalCameraData>().SetRenderer(1);
        cameraa.GetComponent<CameraFocus>().hasGlassesOn = true;
    }
    
    void Update()
    {
        if(Buttons.IsButtonUp(Buttons.KeyItem))
        {
            cameraa.GetComponent<CameraFocus>().hasGlassesOn = false;
            cameraa.GetComponent<UniversalAdditionalCameraData>().SetRenderer(0);
            Destroy(gameObject);

        }
    }
}
