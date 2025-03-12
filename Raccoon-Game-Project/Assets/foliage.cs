using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foliage : MonoBehaviour
{
    [SerializeField] float intensity;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = FindFirstObjectByType<Camera>().transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Transform camera = FindFirstObjectByType<Camera>().transform;
        transform.position = camera.position / intensity;
        transform.position = new Vector3(transform.position.x, transform.position.y, 10);
        transform.position = Vector3Int.RoundToInt(transform.position * GameDefinitions.UNIT_PIXELS);
        transform.position = transform.position / GameDefinitions.UNIT_PIXELS;
    }
}
