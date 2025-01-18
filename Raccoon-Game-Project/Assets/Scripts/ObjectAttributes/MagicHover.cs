using UnityEngine;
[RequireComponent(typeof(Heightable))]
public class MagicHover : MonoBehaviour
{
    Heightable heightable;
    CameraFocus cam;
    public float distanceOffGround;
    // Start is called before the first frame update
    void Start()
    {
        heightable = GetComponent<Heightable>();
        cam = FindAnyObjectByType<CameraFocus>();
        distanceOffGround = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {

        if (!cam.IsOnScreen(transform.position))
            return;
        heightable.height = (Mathf.Sin(Time.time * 4) / 4) + distanceOffGround;
    }
}
