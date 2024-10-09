using UnityEngine;

public class ScrollingClouds : MonoBehaviour
{
    Vector2 scroll = new Vector2(0, 0);
    Vector2 direction = new Vector2(0.5f, 0.25f);
    float distance = 10;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = FindFirstObjectByType<Camera>().transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Transform camera = FindFirstObjectByType<Camera>().transform;
        scroll += direction * Time.deltaTime;
        scroll.x %= 16;
        scroll.y %= 16;
        transform.position = (Vector3)scroll + (camera.position / 2);
        transform.position = new Vector3(transform.position.x, transform.position.y, distance);
        transform.position = Vector3Int.RoundToInt(transform.position * 16);
        transform.position = transform.position / 16;

    }
}
