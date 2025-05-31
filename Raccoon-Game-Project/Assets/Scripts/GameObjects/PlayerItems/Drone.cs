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
    Inventory inventory; //for if we change items.

    AudioSource flySound;
    // Start is called before the first frame update
    void Start()
    {
        CameraFocus cameraFocus = FindFirstObjectByType<CameraFocus>();
        cameraFocus.SetTarget(transform);
        rb = GetComponent<Rigidbody2D>();
        heightable = GetComponent<Heightable>();
        player = FindAnyObjectByType<PlayerStateManager>();
        player.SwitchState(new NoInputPlayerState());
        inventory = FindFirstObjectByType<Inventory>();
        flySound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(isReturning)
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime*10);
            heightable.height = Mathf.Lerp(heightable.height, 0, Time.deltaTime*10);
            flySound.pitch = Mathf.Clamp(Vector2.Distance(transform.position, player.transform.position)/10, 0.75f, 1.5f);
            if(Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                player.SwitchState(new DefaultPlayerState());
                FindFirstObjectByType<CameraFocus>().SetTarget(player.transform);
                Destroy(gameObject);
            }
        }
        else
        {
            if(Buttons.IsButtonDown(Buttons.KeyItem) || inventory.isOn || player.currentPlayerState is not NoInputPlayerState) //fixes for drone being enabled.
            {
                isReturning = true;
            }

            //set height based on animCurve
            timer += Time.deltaTime;
            if(timer > curve.keys.Last().time)
            {
                timer = curve.keys[1].time;
                flySound.pitch = Mathf.Clamp(rb.linearVelocity.magnitude/4f, 0.75f, 1.5f);
            }
            else
            {
                flySound.pitch = Mathf.Clamp(heightable.height, 0.75f, 1.5f);
            }
            heightable.height = curve.Evaluate(timer)*HEIGHT;

            //position and rotation
            transform.GetChild(0).rotation = Quaternion.Euler(0,0,Rotation.DirectionToAngle(rb.linearVelocity));
            rb.AddForce(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))*40);
            
        }
        
        
    }
}
