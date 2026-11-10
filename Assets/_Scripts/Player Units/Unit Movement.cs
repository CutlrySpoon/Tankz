using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitMovement : MonoBehaviour
{
    public float speed = 5f;
    public LayerMask layermask;

    public float maxFallSpeed = 15f;
    private Vector2 moveInput;
    private Vector2 rawInput;
    private float verticalVelocity;
    [HideInInspector] public bool grounded;
    private float gravity = 9.81f;
    public float jumpHeight = 8f;
    private bool jumpUp;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [HideInInspector] public bool isBlue;
    private RaycastHit hit;

    void Awake()
    {
        if (GameManager.gm.turnCount % 2 == 0)
        {
            isBlue = true;
        }
        else
        {
            isBlue = false;
        }
        layermask = LayerMask.GetMask("Ground");
    }

    void FixedUpdate()
    {
        if(!Physics.Raycast(transform.position, Vector3.down, out hit, 0.1f, layermask) && !PointerController.Instance.selectedUnit == this.gameObject)
        {
            grounded = false;
        }
        if (GetComponent<WeaponControler>().isCharging == true || PointerController.Instance.selectedUnit != this.gameObject)
        {
            moveInput = Vector2.zero;
        }
        //jump if grounded
        if (jumpUp && (grounded))
        {
            verticalVelocity = jumpHeight;
            grounded = false;
            jumpUp = false;
        }
        // Apply gravity if not grounded
        else if (!grounded)
        {
            verticalVelocity -= gravity * Time.fixedDeltaTime;
            verticalVelocity = Mathf.Max(verticalVelocity, -maxFallSpeed);
        }
        //Death border
        if (transform.position.y <= -12f)
        {
            UnitHealth unitHealth = GetComponent<UnitHealth>();
            unitHealth.TakeDamage(9999, false);
        }

        Vector3 finalMovement = new Vector3(moveInput.x * speed, verticalVelocity, 0f) * Time.fixedDeltaTime;
        transform.Translate(finalMovement, Space.World);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & layermask) != 0)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                // Ground check
                if (contact.normal.y > 0.5f)
                {
                    grounded = true;
                    if (verticalVelocity < 0)
                    {
                        verticalVelocity = 0f;
                    }
                }
                // Ceiling check
                else if (contact.normal.y < -0.5f)
                {
                    if (verticalVelocity > 0)
                    {
                        verticalVelocity = 0f;
                    }
                }
                // Wall checks 
                Vector2 rawInput = moveInput;
                if (contact.normal.x > 0.5f && rawInput.x < 0)
                {
                    // Block left movement into a right-facing wall
                    transform.Translate(new Vector3(0.01f, 0f, 0f), Space.World);
                    moveInput.x = 0f;
                }
                else if (contact.normal.x < -0.5f && rawInput.x > 0)
                {

                    // Block right movement into a left-facing wall
                    transform.Translate(new Vector3(-0.01f, 0f, 0f), Space.World);
                    moveInput.x = 0f;
                }

            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        moveInput = rawInput;
        if (((1 << collision.gameObject.layer) & layermask) != 0)
        {
            if(Physics.Raycast(transform.position, Vector3.down, out hit, 0.1f, layermask) == false)
            {
                grounded = false;
            }
            else
            {
                grounded = true;
            }
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        Debug.Log(PointerController.Instance.selectedUnit);
        Debug.Log(GetComponent<WeaponControler>().shotqueued);
        Debug.Log(GameManager.gm.blueTurn);
        if (PointerController.Instance.selectedUnit == this.gameObject && GetComponent<WeaponControler>().shotqueued == false && isBlue == GameManager.gm.blueTurn)
        {

            rawInput = context.ReadValue<Vector2>();
            moveInput = new Vector2(rawInput.x, 0f);
            if(rawInput.x > 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (rawInput.x < 0)
            {
                spriteRenderer.flipX = false;
            }
            if (rawInput.y > 0 && (context.started || context.performed))
            {
                if (grounded && GetComponent<WeaponControler>().isCharging == false)
                {
                    jumpUp = true;
                }
            }
        }
    }
}