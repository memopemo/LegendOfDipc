using System.Collections;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    [SerializeField] Collider2D bounds;
    public Transform target;
    [SerializeField] Vector2 offset;
    const float distance = GameDefinitions.SCREEN_HEIGHT_PIXELS / 2f / GameDefinitions.UNIT_PIXELS; //units from the middle of the screen to the bottom/top. the full height of the camera box is 2*distance.
    public float shakeIntensity;

    [SerializeField] float followSpeed;
    [SerializeField] float lookaheadTime;

    Vector2 playerLookAhead;
    PlayerStateManager player;
    const float ORTHO_CAMERA_DISTANCE = 10;
    Vector2 lookaheadVelocity;

    public bool hasGlassesOn;
#if DEBUG
    bool noScrolling = false;
#endif
    bool lerpToNewBoundsActive = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (!target)
        {
            player = FindFirstObjectByType<PlayerStateManager>();
            Debug.Assert(player, "No Player Found to focus Camera on.");
            if (player)
            {
                target = player.transform;
            }
            else
            {

                target = new GameObject("No Player").transform;
            }

        }
    }

    //Set position so the camera does not fly over to the player from its original position in the editor when the scene starts.
    public void InitializeCameraPosition(Vector3 startingDirection)
    {
        if (target)
            transform.position = target.position + startingDirection;
    }
    void OnValidate()
    {
        Camera _camera = GetComponent<Camera>();
        _camera.orthographicSize = distance;
        if (bounds != null && !bounds.bounds.Contains(transform.position))
        {
            Vector2 _closest = bounds.ClosestPoint(transform.position);
            if (bounds.bounds.extents == Vector3.zero) _closest = bounds.bounds.center;
            transform.position = new Vector3(_closest.x, _closest.y, transform.position.z);
        }

    }

    // Update is called once per frame
    void LateUpdate()
    {
#if DEBUG
        if (Input.GetKeyDown(KeyCode.ScrollLock))
        {
            noScrolling = !noScrolling;
        }
        if (noScrolling) return;
#endif
        //set camera properties
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, distance, Time.deltaTime * followSpeed);

        //get player
        if (player != null)
        {
            if (target.gameObject == player.gameObject)
            {
                playerLookAhead = Vector2.SmoothDamp(playerLookAhead, player.directionedObject.direction, ref lookaheadVelocity, lookaheadTime);
            }
            else
            {
                playerLookAhead = Vector2.SmoothDamp(playerLookAhead, Vector2.zero, ref lookaheadVelocity, followSpeed);
            }
        }



        if (lerpToNewBoundsActive && !bounds.bounds.Contains(transform.position))
        {
            Vector2 _closest = bounds.ClosestPoint(transform.position);
            if (bounds.bounds.extents == Vector3.zero) _closest = bounds.bounds.center;
            transform.position = Vector3.Lerp(transform.position, _closest, Time.deltaTime * followSpeed * 4);
            transform.position = new Vector3(transform.position.x, transform.position.y, -ORTHO_CAMERA_DISTANCE);
            return; // skip player lerp.
        }
        else
        {
            lerpToNewBoundsActive = false;
        }

        //position camera with regards to previous position.
        transform.position = Vector3.Lerp(transform.position, target.position + (Vector3)offset + (Vector3)playerLookAhead, Time.deltaTime * followSpeed);

        transform.position = new Vector3(transform.position.x, transform.position.y, -ORTHO_CAMERA_DISTANCE);

        //put in bounds
        if (bounds && !bounds.bounds.Contains(transform.position))
        {
            Vector2 _closest = bounds.ClosestPoint(transform.position);
            if (bounds.bounds.extents == Vector3.zero) _closest = bounds.bounds.center;
            transform.position = new Vector3(_closest.x, _closest.y, transform.position.z);
        }

        //add shake
        if (shakeIntensity > 0)
        {
            transform.position += (Vector3)Random.insideUnitCircle.normalized * (shakeIntensity / 10);
            shakeIntensity -= Time.deltaTime * 5;
        }
        else shakeIntensity = 0;

    }
    public void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.position * Vector2.one, 0.25f);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
    public void ShakeScreen(float strength = 1)
    {
        if (shakeIntensity > strength) return;
        shakeIntensity = strength;
    }
    public bool IsOnScreen(Vector2 position)
    {
        Rect rect = new(0, 0, distance * 2 * Camera.main.aspect, distance * 2) //make new rect with correct aspect ratio.
        {
            center = transform.position
        };
        return rect.Contains(position);
    }
    public void ChangeBounds(Collider2D newBounds)
    {
        bounds = newBounds;
        lerpToNewBoundsActive = true;
        Invoke(nameof(RemoveLerp), 1f);
    }
    void RemoveLerp()
    {
        lerpToNewBoundsActive = false;
    }
}
