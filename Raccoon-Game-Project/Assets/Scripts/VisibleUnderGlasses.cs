using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class VisibleUnderGlasses : MonoBehaviour
{
    Renderer rendererFuck2010AssUnityisms;
    CameraFocus cameraFocus;
    // Start is called before the first frame update
    void Start()
    {
        rendererFuck2010AssUnityisms = GetComponent<Renderer>();
        cameraFocus = FindFirstObjectByType<CameraFocus>();

    }
    void Update()
    {
        rendererFuck2010AssUnityisms.enabled = cameraFocus.hasGlassesOn;
    }
}
