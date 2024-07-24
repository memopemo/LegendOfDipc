using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    [SerializeField] Collider2D bounds;
    public Transform target;
    [SerializeField] Vector2 offset;
    [SerializeField] float distance;
    public float shakeIntensity;

    [SerializeField] float followSpeed;
    [SerializeField] float lookaheadTime;

    Vector2 playerLookAhead;
    const float ORTHO_CAMERA_DISTANCE = 10;
    Vector2 lookaheadVelocity;
    #if DEBUG
    float debugKeyHeldTime = 0;
    #endif

    // Start is called before the first frame update
    void Start()
    {
        
        if (!target)
        {
            PlayerStateManager player = FindFirstObjectByType<PlayerStateManager>();
            Debug.Assert(player, "No Player Found to focus Camera on.");
            if(player)
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
        if(target)
            transform.position = target.position + startingDirection;
    }
    void OnValidate()
    {
        Camera _camera = GetComponent<Camera>();
        _camera.orthographicSize = distance;
        if (bounds != null && !bounds.bounds.Contains(transform.position))
        {
            Vector2 _closest = bounds.ClosestPoint(transform.position);
            transform.position = new Vector3(_closest.x, _closest.y, transform.position.z);
        }

    }

    // Update is called once per frame
    void LateUpdate()
    {
        //set camera properties
        Camera _camera = GetComponent<Camera>();
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, distance, Time.deltaTime * followSpeed);

        //get player
        PlayerStateManager CurrentPlayer = FindFirstObjectByType<PlayerStateManager>();
        if(CurrentPlayer != null)
        {
            if (target.gameObject == CurrentPlayer.gameObject)
            {
                playerLookAhead = Vector2.SmoothDamp(playerLookAhead, CurrentPlayer.directionedObject.direction, ref lookaheadVelocity, lookaheadTime);
            }
            else
            {
                playerLookAhead = Vector2.SmoothDamp(playerLookAhead, Vector2.zero, ref lookaheadVelocity, followSpeed);
            }
        }

        //position camera with regards to previous position.
        transform.position = Vector3.Lerp(transform.position, target.position + (Vector3)offset + (Vector3)playerLookAhead, Time.deltaTime * followSpeed);

        transform.position = new Vector3(transform.position.x, transform.position.y, -ORTHO_CAMERA_DISTANCE);

        //put in bounds
        if (bounds && !bounds.bounds.Contains(transform.position))
        {
            Vector2 _closest = bounds.ClosestPoint(transform.position);
            transform.position = new Vector3(_closest.x, _closest.y, transform.position.z);
        }

        //add shake
        if (shakeIntensity > 0)
        {
            transform.position += (Vector3)Random.insideUnitCircle.normalized * (shakeIntensity / 10);
            shakeIntensity -= Time.deltaTime * 5;
        }
        else shakeIntensity = 0;
        #if DEBUG
        if(Input.GetKey(KeyCode.Period) || Input.GetKey(KeyCode.Comma) )
        {
            debugKeyHeldTime += Time.deltaTime*2;
        }
        else
        {
            debugKeyHeldTime = 1;
        }
        //debug zooming
        if(Input.GetKey(KeyCode.Period))
        {
            distance += Time.unscaledDeltaTime*debugKeyHeldTime;
        }
        if(Input.GetKey(KeyCode.Comma))
        {
            distance -= Time.unscaledDeltaTime*debugKeyHeldTime;
        }
        #endif
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
        if(shakeIntensity > strength) return;
        shakeIntensity = strength;
    }
}
