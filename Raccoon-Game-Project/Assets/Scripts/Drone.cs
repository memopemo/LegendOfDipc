using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Drone : MonoBehaviour
{
    [SerializeField] AnimationCurve curve;
    float timer;
    Heightable heightable;
    Rigidbody2D rb;
    public static int HEIGHT = 4; //used like a const.
    public bool isReturning;
    public PlayerStateManager player;
    // Start is called before the first frame update
    void Start()
    {
        CameraFocus cameraFocus = FindFirstObjectByType<CameraFocus>();
        cameraFocus.SetTarget(transform);
        rb = GetComponent<Rigidbody2D>();
        heightable = GetComponent<Heightable>();
        Invoke(nameof(LateStart),0.5f);
    }
    void LateStart()
    {
        player = FindAnyObjectByType<PlayerStateManager>();
        player.SwitchState(new NoInputPlayerState());
    }

    // Update is called once per frame
    void Update()
    {
        
        if(isReturning)
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime*10);
            heightable.height = Mathf.MoveTowards(heightable.height, 0, Time.deltaTime*10);
            if(Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                player.SwitchState(new DefaultPlayerState());
                FindFirstObjectByType<CameraFocus>().SetTarget(player.transform);
                Destroy(gameObject);
            }
        }
        else
        {
            if(Input.GetButtonDown("Fire2"))
            {
                isReturning = true;
            }

            //set height based on animCurve
            timer += Time.deltaTime;
            if(timer > curve.keys.Last().time)
            {
                timer = curve.keys[1].time;
            }
            heightable.height = curve.Evaluate(timer)*HEIGHT;

            //position and rotation
            transform.GetChild(0).rotation = Quaternion.Euler(0,0,Rotation.DirectionToAngle(rb.velocity));
            rb.AddForce(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))*40);
        }
    
    }
}
