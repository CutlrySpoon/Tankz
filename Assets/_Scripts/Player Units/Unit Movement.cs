using UnityEngine;
using UnityEngine.InputSystem;
using static GameManager;

// Tank movement

// By JM and OS

public class UnitMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpHeight = 8f;
    [SerializeField] private float maxFallSpeed = 15f;
    [Space(5)]

    [Header("References")]
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer barrelSprite;


    private Rigidbody rb;

    private const RigidbodyConstraints mobileCons = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
    private const RigidbodyConstraints parkedCons = mobileCons | RigidbodyConstraints.FreezePositionX;

    private Collider col;
    private WeaponControler weapon;

    public bool grounded { get; private set; }
    public bool isBlue { get; private set; }
    private bool jumpQueued;
    private bool wasMobile;

    private Vector2 moveInput;
    private Vector2 rawInput;

    private Vector3 velocity
    {
        get => rb.linearVelocity;
        set => rb.linearVelocity = value;
    }

    // 
    void Awake()
    {
        isBlue = gm.turnCount % 2 == 0;

        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        weapon = GetComponent<WeaponControler>();

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.constraints = parkedCons;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    void FixedUpdate()
    {
        bool selected = PointerController.Instance.selectedUnit == gameObject;
        bool canMove = selected && !weapon.isCharging;

        // Prevent movement of non selected tanks
        if (canMove != wasMobile)
        {
            rb.constraints = canMove ? mobileCons : parkedCons;
            wasMobile = canMove;
        }

        if (!canMove)
        {
            moveInput = Vector2.zero;
            jumpQueued = false;
        }

        Vector3 v = velocity;
        v.x = canMove ? moveInput.x * speed : 0f;
        v.z = 0f;

        if (jumpQueued && grounded)
        {
            v.y = jumpHeight;
            jumpQueued = false;
        }

        v.y = Mathf.Max(v.y, -maxFallSpeed);
        velocity = v;

        grounded = false;

        // Death Border
        if (transform.position.y <= -12f)
        {
            UnitHealth unitHealth = GetComponent<UnitHealth>();
            unitHealth.TakeDamage(9999, false);
        }
    }

    // Grounded check
    private void OnCollisionStay(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            if (collision.GetContact(i).normal.y > 0.5f)
            {
                grounded = true;
                return;
            }
        }
    }

    // Movement and jumping
    public void Move(InputAction.CallbackContext context)
    {
        if (PointerController.Instance.selectedUnit != gameObject || weapon.hasShot || isBlue != gm.blueTurn) return;

        Vector2 rawInput = context.ReadValue<Vector2>();
        moveInput = new Vector2(rawInput.x, 0f);

        if (rawInput.x > 0) spriteRenderer.flipX = true;
        else if (rawInput.x < 0) spriteRenderer.flipX = false;

        if (rawInput.y > 0 && (context.started || context.performed) && grounded && !weapon.isCharging)
        {
            jumpQueued = true;
        }

    }
}