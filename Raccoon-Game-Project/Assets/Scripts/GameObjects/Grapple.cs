using Unity.Mathematics;
using UnityEngine;

//Launches a roll of measuring tape that can grapple certain grappleable objects (blocks and pickups)
public class Grapple : MonoBehaviour
{
    private const float DECELERATION = 0.2f;
    private const int START_SPEED = 32;
    private SpriteRenderer tape;
    private float length;
    private float speed;
    int frameCount;
    [SerializeField] private Vector2 direction;
    private Vector2 upTapePosition = new(0, 0.375f);
    private Vector2 downTapePosition = new(0, -0.1875f);
    private Vector2 rightTapePosition = new(0.25f, -0.2185f); //left we just *-1 the x.
    [SerializeField] private Sprite tapeSideSprite;
    [SerializeField] private Sprite tapeUpSprite;
    [SerializeField] private Sprite tapeDownSprite;
    [SerializeField] private Sprite debugCollision;
    private bool isAttaching; //drag player along if we found something to grapple onto.
    private Transform hookPoint;
    private PlayerStateManager player;
    [SerializeField] private GameObject hitParticle;
    private NoiseMaker noiseMaker;
    const int SND_EXTEND = 0;
    const int SND_HIT = 1;
    const int SND_LATCH = 2;
    const int SND_RETRACT = 3;
    const int SND_RETRACT_FINISH = 4;
    const int PLAYER_ANIM = 28;
    private const int ATTACH_REACTION_SPEED = 7;
    bool alreadyPlayingRetractSound;

    private void Start()
    {
        tape = transform.GetChild(0).GetComponent<SpriteRenderer>();
        hookPoint = transform.GetChild(1);
        player = FindFirstObjectByType<PlayerStateManager>();
        direction = GetComponent<DirectionedObject>().direction = player.directionedObject.direction;
        noiseMaker = GetComponent<NoiseMaker>();
        transform.position -= (Vector3)direction / 2;

        //check around player if we are like, close to a wall. Update() will update the hookpoint afterwards.
        length = 0.1f;
        hookPoint.position = player.transform.position + (Vector3)direction/2;
        player.SwitchState(new GrapplePlayerState(player, transform));
        noiseMaker.Play(SND_EXTEND);
        CheckSurrounding();
        //initialize tape position, sprite, and flip based on direction.
        InitializeTapeSprite();

        isAttaching = false;
        speed = START_SPEED;
        
        //print(Rotation.DirectionToAngle(direction));
    }

    private void InitializeTapeSprite()
    {
        if (direction.x != 0 && direction.y == 0)
        {
            tape.transform.localPosition = new Vector2(rightTapePosition.x * direction.x, rightTapePosition.y);
            tape.sprite = tapeSideSprite;
        }
        else if (direction == Vector2Int.up)
        {
            tape.transform.localPosition = upTapePosition;
            tape.sprite = tapeUpSprite;
        }
        else if (direction == Vector2Int.down)
        {
            tape.transform.localPosition = downTapePosition;
            tape.sprite = tapeDownSprite;
        }
        tape.flipX = direction.x > 0;
        tape.flipY = direction.y > 0;
        tape.size = tape.sprite.bounds.size;
        
        // put sprite in front of tape if we are facing down.
        if (direction != Vector2Int.down)
        {
            tape.transform.localPosition += Vector3.forward * 0.2f;
        }
        tape.enabled = false;
    }

    private void FixedUpdate()
    {
        if(length < 0)
        {
            TapeDone();
        }
        frameCount++;
        if(frameCount < 8) return;
        else
        {
            tape.enabled = true;
        }
        //update vars
        speed -= DECELERATION; //speed will change over time=
        length += Time.deltaTime * speed; //this will go negative speed (going backwards.)
        
        if (isAttaching)
        {
            transform.position = hookPoint.position - GetDirectionedLength();
        }

        //set tape length based on direction
        if (direction.x != 0)
        {
            tape.size = new Vector2(-length, tape.size.y);
        }
        else
        {
            tape.size = new Vector2(tape.size.x, length);
        }
        if(speed < 0)
        {
            PlayRetractSound();
        }


        //set end of tape position.
        hookPoint.position = transform.position + GetDirectionedLength();

        //update collision with tape if we havent failed.
        if(!isAttaching && speed > 0)
        {
            UpdateCheckCollide();
        }
    }

    private void TapeDone()
    {
        hookPoint.DetachChildren();
        player.SwitchState(new DefaultPlayerState());
        noiseMaker.Play(SND_RETRACT_FINISH, true);
        Destroy(gameObject);
    }

    private void UpdateCheckCollide()
    {
        Vector2 box = (new Vector2(Mathf.Abs(direction.x), Mathf.Abs(direction.y)) / 2) + (Vector2.one / 3); //shape is an oblong rectangle either horizontally (for l/r) or vertically (u/d)
        RaycastHit2D[] collisions = Physics2D.BoxCastAll(hookPoint.position, box, 0, Vector2.zero); 
        foreach (RaycastHit2D ray in collisions)
        {
            if (ray.collider.gameObject == gameObject) continue; //ignore ourselves
            if (ray.collider.TryGetComponent(out PlayerStateManager _)) continue; //ignore player.
            if (ray.collider.isTrigger) continue; //ignore uncollideable things
            if (ray.collider.TryGetComponent(out Grappleable obj) && obj.isHookPoint)
            {
                if(obj.isHookPoint)
                {
                    isAttaching = true;
                    speed = ATTACH_REACTION_SPEED;
                    transform.parent = obj.transform;
                    noiseMaker.Play(SND_LATCH, true);
                    PlayRetractSound();
                    (player.currentPlayerState as GrapplePlayerState).StartDragAnimation(player);
                    transform.position = ray.collider.transform.position - (Vector3)(direction * length);
                    return;
                }
                else
                {
                    obj.transform.parent = hookPoint.transform;
                }
            }
            speed *= -1;
            speed -= 1;
            //print(ray.collider.gameObject.name);
            noiseMaker.Play(SND_HIT, true);
            PlayRetractSound();
            Instantiate(hitParticle, hookPoint.position, Quaternion.Euler(0,0,Vector2.SignedAngle(Vector2.down, direction)));
            return;
        }
    }
    private void CheckSurrounding()
    {
        Vector2 box = (new Vector2(Mathf.Abs(direction.x), Mathf.Abs(direction.y)) / 2) + (Vector2.one / 3); //shape is an oblong rectangle either horizontally (for l/r) or vertically (u/d)
        RaycastHit2D[] collisions = Physics2D.BoxCastAll(hookPoint.position, box, 0, Vector2.zero); 
        foreach (RaycastHit2D ray in collisions)
        {
            if (ray.collider.gameObject == gameObject) continue; //ignore ourselves
            if (ray.collider.TryGetComponent(out PlayerStateManager _)) continue; //ignore player.
            if (ray.collider.isTrigger) continue; //ignore uncollideable things
            noiseMaker.Play(SND_HIT, true);
            Instantiate(hitParticle, hookPoint.position, Quaternion.Euler(0,0,Vector2.SignedAngle(Vector2.down, direction)));
            TapeDone();
            return;
        }
    }

    private void PlayRetractSound()
    {
        if(alreadyPlayingRetractSound) return;
        noiseMaker.Play(SND_RETRACT);
        alreadyPlayingRetractSound = true;
    }

    private Vector3 GetDirectionedLength() => direction * length;
}
