using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugIsOnScreen : MonoBehaviour
{
    public Sprite yes;
    public Sprite no;
    CameraFocus cameraFocus;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        cameraFocus = FindFirstObjectByType<CameraFocus>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.sprite = cameraFocus.IsOnScreen(transform.position) ? yes : no;
    }
}
