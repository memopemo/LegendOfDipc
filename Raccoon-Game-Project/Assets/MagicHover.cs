using UnityEngine;
[RequireComponent(typeof(Heightable))]
public class MagicHover : MonoBehaviour
{
    Heightable heightable;
    CameraFocus camera;
    // Start is called before the first frame update
    void Start()
    {
        heightable = GetComponent<Heightable>();
        camera = FindAnyObjectByType<CameraFocus>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!camera.IsOnScreen(transform.position)) 
            return;
        heightable.height = (Mathf.Sin(Time.time * 4) / 4) + 0.5f;
    }
}
