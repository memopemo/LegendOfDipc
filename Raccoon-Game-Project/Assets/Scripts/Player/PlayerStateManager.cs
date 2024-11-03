using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D), typeof(DirectionedObject))]
[RequireComponent(typeof(SpriteRenderer), typeof(Animator2D.Animator2D))]
public class PlayerStateManager : MonoBehaviour
{
    //* Components *//
    [NonSerialized] public Rigidbody2D rigidBody;
    [NonSerialized] public SpriteRenderer defaultSpriteRenderer;
    [NonSerialized] public DirectionedObject directionedObject;
    //[NonSerialized] public SpriteRenderer jumpSpriteRenderer;
    [NonSerialized] public Heightable height;
    [NonSerialized] public Animator2D.Animator2D animator;
    [NonSerialized] public NoiseMaker noiseMaker;
    //* General Variables *//
    public IPlayerState currentPlayerState;
    public float additionalSpeed;
    public GameObject poofParticle;
    public GameObject splashParticle;
    public GameObject healParticle;
    public GameObject swordHitBoxObject;
    public GameObject boomerang;
    public GameObject deathScreen;
    private float knockBackTimer;
    private Vector2 knockBackDirection;
    /* This is where the player should be placed after they fall into a pit.
     * Only updated when entering certain triggers that set this. For example between dungeon rooms, or a straight path towards water.
     */
    public Vector2 FallReturnPosition;

    public ItemList itemGameObjectLookup;


    //* Helpful Variables *//
    //* These are for the different states to use periodically. *//
    [NonSerialized] public Vector2 rawInput;
    [NonSerialized] public float inputX;
    [NonSerialized] public float inputY;
    public IPlayerState previousState { get; private set; }
    [NonSerialized] public int stateTransitionTimer1; // for transition wait periods between states, (like landing after jumping or smth)
    [NonSerialized] public int stateTransitionTimer2;
    [NonSerialized] public int generalTimer1;
    [NonSerialized] public int generalTimer2;
    CollisionCheck spaceCheck;
    public CollisionCheck directionCheck;
    public CollisionCheck waterCheck;

#if DEBUG
    //* Debug Variables *//

    // This helps with going between states automatically.
    Dictionary<KeyCode, IPlayerState> debugNumpadKeysToState = new()
        {
            { KeyCode.Keypad0,      new DefaultPlayerState()        },
            { KeyCode.Keypad1,      new SwimPlayerState()           },
            { KeyCode.Keypad2,      new SnorkelPlayerState()        },
            { KeyCode.Keypad3,      new RammingPlayerState()        },
            { KeyCode.Keypad4,      new ClimbingPlayerState(-5,5)   },
            { KeyCode.KeypadEnter,  new DebugFreeRoamPlayerState()  },
            { KeyCode.KeypadPeriod, new NoInputPlayerState()        },
        };
#endif
    // Awake is called during initialization of a script. Use for setting up references to gameobjects and components before sending/reading data from them.
    void Awake()
    {
        //assuming initial place is a safe spot.
        FallReturnPosition = transform.position;

        //Component Init
        rigidBody = GetComponent<Rigidbody2D>();
        defaultSpriteRenderer = GetComponent<SpriteRenderer>();
        //jumpSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        height = GetComponent<Heightable>();
        directionedObject = GetComponent<DirectionedObject>();
        animator = GetComponent<Animator2D.Animator2D>();
        noiseMaker = GetComponent<NoiseMaker>();
        spaceCheck = new(GetComponent<Collider2D>());
        spaceCheck
            .SetType(CollisionCheck.CollisionType.Circle)
            .SetCircleRadius(1.5f)
            .SetDebug(true);
        directionCheck = new(GetComponent<Collider2D>());
        directionCheck
            .MoveFromDirection(0.5f, directionedObject)
            .SetDragPositionFromDirection(0.5f, directionedObject)
            .SetType(CollisionCheck.CollisionType.DraggedBox)
            .SetFindTriggers(false)
            .SetDebug(true)
            .SetBoxSize(0.51f);
        waterCheck = new(GetComponent<Collider2D>());
        waterCheck
            .SetFindTriggers(true)
            .SetZRange(transform.position.z, transform.position.z + 3)
            .SetRelativePosition(Vector2.down * 0.2f)
            .SetType(CollisionCheck.CollisionType.Point);

    }

    // Start is called before the first frame update
    void Start()
    {
        //State Init
        currentPlayerState = new DefaultPlayerState();
        currentPlayerState.OnEnter(this);
        InvokeRepeating(nameof(AttemptToSetSafePosition), 2, 2);
    }

    //Set player at specified place with direction on scene load.
    public void RoomInitialize(Vector2 position, Vector2Int direction)
    {
        transform.position = position;
        GetComponent<DirectionedObject>().direction = direction;
    }

    // Update is called once per frame
    void Update()
    {
        // Get input
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        rawInput = new Vector2(inputX, inputY);

        // Update timers
        Timer.DecrementTimer(ref stateTransitionTimer1);
        Timer.DecrementTimer(ref stateTransitionTimer2);
        Timer.DecrementTimer(ref generalTimer1);
        Timer.DecrementTimer(ref generalTimer2);


        if (DemonBuffs.HasBuff(DemonBuffs.DemonBuff.Speed0))
        {
            additionalSpeed = 1;
        }
        else if (DemonBuffs.HasBuff(DemonBuffs.DemonBuff.Speed1))
        {
            additionalSpeed = 2;
        }
        else
        {
            additionalSpeed = 0;
        }

        directionCheck
            .MoveFromDirection(0.5f, directionedObject)
            .SetDragPositionFromDirection(0.5f, directionedObject);

        // Switch State
        currentPlayerState.OnUpdate(this);
        DemonBuffs.Update();


#if DEBUG
        //debug
        foreach (var x in debugNumpadKeysToState)
        {
            if (Input.GetKeyDown(x.Key))
            {
                SwitchState(x.Value);
            }
        }
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            PlayerHealth.Heal(1, this);
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            PlayerHealth.TakeDamage(1, this);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            PlayerHealth.debugLockHealth ^= true;
            PlayerHealth.SetHealth(int.MaxValue);
        }
        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            if (TryGetComponent(out FloorLevel fl))
            {
                fl.IncrementLevel(1);
            }
        }
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            if (TryGetComponent(out FloorLevel fl))
            {
                fl.DecrementLevel(1);
            }
        }
#endif

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckDamage(collision.gameObject);
    }
    private void CheckDamage(GameObject go)
    {
        //if enemy, take damage.
        if (go.TryGetComponent(out DamagesPlayer hurtful))
        {
            AttemptToTakeDamage(hurtful);
        }
    }
    public void AttemptToTakeDamage(DamagesPlayer hurtful)
    {
        if (height.height > 0) return;
        if (currentPlayerState is HurtPlayerState or HeldItemPlayerState or RammingPlayerState or CloakedPlayerState) return;
        if (currentPlayerState is ShieldPlayerState && DoesShieldBlockDamage(hurtful)) return;
        TakeDamage(hurtful);
    }
    public bool DoesShieldBlockDamage(DamagesPlayer hurtful)
    {
        //this is so if we are still touching it when we stop shielding, we get hurt immediately.
        (currentPlayerState as ShieldPlayerState).AddDamagesPlayer(hurtful);

        //get direction of hurt (think of an arrow drawn from hurtful to player)
        Vector2Int directionOfHurt = Vector2Int.RoundToInt((transform.position - hurtful.transform.position).normalized);
        Vector2Int direction = directionedObject.direction;

        //are we facing close to the opposite direction of the attack? (allow 45 degree edge cases.)
        bool xPortion = directionOfHurt.x != 0 && direction.x != 0 && directionOfHurt.x == -direction.x;
        bool yPortion = directionOfHurt.y != 0 && direction.y != 0 && directionOfHurt.y == -direction.y;

        return xPortion || yPortion;
    }
    public void TakeDamage(DamagesPlayer hurtful)
    {
        PlayerHealth.TakeDamage(HitCalculation.HurtPlayerAmount(hurtful.amount), this);
        GetComponent<Flasher>().StartFlash();
        Knockback(hurtful.transform);
        stateTransitionTimer1 = 25; //25 frames of knockback.
        SwitchState(new HurtPlayerState());
        return;
    }
    public void Knockback(Transform from)
    {
        if (knockBackTimer > 0) return;

        knockBackDirection = (transform.position - from.position).normalized;
        directionedObject.direction = Vector2Int.RoundToInt(-knockBackDirection);

        rigidBody.AddForce(knockBackDirection * 3, ForceMode2D.Impulse);
    }

    // State Machine
    public void SwitchState(IPlayerState state)
    {
        previousState = currentPlayerState;
        currentPlayerState.OnLeave(this);
        currentPlayerState = state;
        currentPlayerState.OnEnter(this);
    }

    // After falling
    public void SetPlayerBack()
    {
        PlayerHealth.TakeDamage(1, this);
        transform.position = FallReturnPosition;
        SwitchState(new DefaultPlayerState());
    }

    // If this is a safe spot to put the player back after falling down a pit, set our position here.
    // Definition of a safe spot:
    //      1. No colliders within a circle 3 blocks diameter
    //      2. Not doing anything except running or idle.
    private void AttemptToSetSafePosition()
    {
        if (currentPlayerState is not DefaultPlayerState) return;
        int tempLayer = gameObject.layer;
        gameObject.layer = 2; // I am like actually retarded. -_- You can just temporarily set your collider to be ignored.
        if (!spaceCheck.EvaluateAnything((c) => { }))
        {
            FallReturnPosition = transform.position;
        }
        gameObject.layer = tempLayer;
    }
    // This is for custom decrementing of an item when it is ready to be used up
    public void DecrementConsumableItem()
    {
        int selectionIndex = SelectedItem.ConsumableItem;
        SaveManager.DecrementConsumableItem(selectionIndex);
    }

    public void DiveIntoDeepWater()
    {
        //this probably shouldnt be my buisness.
        FindFirstObjectByType<CircleFadeInUI>().Out();
        Invoker.InvokeDelayed(() => EnterDeepWaterScene(), 1);
        Cleanuper.ReadyCleanUpForSceneTransition(1);
        SwitchState(new NoInputPlayerState());
    }
    private void EnterDeepWaterScene()
    {
        string underwaterVariantSceneName = SceneManager.GetActiveScene().name + "_Underwater";
        KeepUnderwaterPositionExitHandler.position = transform.position;
        if (SceneUtility.GetBuildIndexByScenePath(underwaterVariantSceneName) == -1)
        {
            SceneManager.LoadScene("error");
            return;
        }
        SceneManager.LoadScene(underwaterVariantSceneName);
    }
    public void DisableSprite()
    {
        Transform sprite = transform.Find("Sprite");
        if (sprite)
            transform.Find("Sprite").GetComponent<SpriteRenderer>().enabled = false;
        else
        {
            Debug.LogWarning("No heightable sprite found.");
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
    public void EnableSprite()
    {
        Transform sprite = transform.Find("Sprite");
        if (sprite)
            transform.Find("Sprite").GetComponent<SpriteRenderer>().enabled = true;
        else
        {
            Debug.LogWarning("No heightable sprite found.");
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    public void Die()
    {
        DisableSprite();
        SwitchState(new NoInputPlayerState());
        rigidBody.velocity = Vector3.zero;
        Instantiate(deathScreen, transform.position, Quaternion.identity);
    }
    /*
    private void OnDrawGizmos()
    {
        //manager.transform.position + (Vector3)(Vector2)manager.directionedObject.direction * 0.5f, Vector2.one * 0.51f, 0, manager.directionedObject.direction, contactFilter, results, 0.5f
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + (Vector3)(Vector2)directionedObject.direction * 0.5f, Vector2.one * 0.51f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube((transform.position + (Vector3)(Vector2)directionedObject.direction * 0.5f) + (Vector3)((Vector2)directionedObject.direction * 0.5f), Vector2.one * 0.51f);
    }
    */
}

