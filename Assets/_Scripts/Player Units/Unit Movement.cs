using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitMovement : MonoBehaviour
{
    public float speed = 5f;
    public LayerMask layermask;

    public float maxFallSpeed = 15f;
    private Vector2 moveInput;
    private float verticalVelocity;
    private bool grounded, grounded2;
    private float gravity = 9.81f; 
    public float jumpHeight = 8f;
    private bool jumpUp;
    public float raycastDistance = 0.55f;
    public float rayOffset = 0.5f;

    void Awake()
    {
        layermask = LayerMask.GetMask("Ground");
    }
    void FixedUpdate()

    {
        //check if the player is grounded by casting a ray down
        RaycastHit hit;


        grounded = Physics.Raycast(transform.position - new Vector3(rayOffset, 0f, 0f), Vector3.down, out hit, raycastDistance, layermask);
        grounded2 = Physics.Raycast(transform.position + new Vector3(rayOffset, 0f, 0f), Vector3.down, out hit, raycastDistance, layermask);
        bool touchingWallLeft = Physics.Raycast(transform.position - new Vector3(0f, rayOffset, 0f), Vector3.left, out hit, raycastDistance, layermask);
        bool touchingWallLeft2 = Physics.Raycast(transform.position + new Vector3(0f, rayOffset, 0f), Vector3.left, out hit, raycastDistance, layermask);
        bool touchingWallRight = Physics.Raycast(transform.position - new Vector3(0f, rayOffset, 0f), Vector3.right, out hit, raycastDistance, layermask);
        bool touchingWallRight2 = Physics.Raycast(transform.position + new Vector3(0f, rayOffset, 0f), Vector3.right, out hit, raycastDistance, layermask);
        bool touchingCeiling = Physics.Raycast(transform.position + new Vector3(rayOffset, 0f, 0f), Vector3.up, out hit, raycastDistance, layermask);
        bool touchingCeiling2 = Physics.Raycast(transform.position - new Vector3(rayOffset, 0f, 0f), Vector3.up, out hit, raycastDistance, layermask);
        if (touchingWallLeft || touchingWallLeft2)
        {
            if(moveInput.x < 0)
            {
                moveInput.x = 0;
            }
        }
        if(touchingWallRight || touchingWallRight2)
        {
            if(moveInput.x > 0)
            {
                moveInput.x = 0;
            }
        }
        if(touchingCeiling || touchingCeiling2)
        {
            if (verticalVelocity > 0)
            {
                verticalVelocity = 0;
            }
        }

        if (grounded || grounded2)
        {
            verticalVelocity = 0f;

            if (jumpUp)
            {
                //if grounded and jump is pressed, set vertical velocity to jump height
                verticalVelocity = jumpHeight;
                grounded = false;
                jumpUp = false;
            }
        }
        else
        {
            //if not grounded, apply gravity to vertical velocity
            verticalVelocity -= gravity * Time.fixedDeltaTime;
            verticalVelocity = Mathf.Max(verticalVelocity, -maxFallSpeed);
        }

        if (!grounded || !grounded2)
        {
            jumpUp = false;
        }

        Vector3 finalMovement = new Vector3(moveInput.x * speed, verticalVelocity, 0f) * Time.fixedDeltaTime;
        transform.Translate(finalMovement, Space.World);

    }

    public void Move(InputAction.CallbackContext context)
    {

        

        Vector2 input = context.ReadValue<Vector2>();
        moveInput = new Vector2(input.x, 0f);
        if (input.y > 0 && (context.started || context.performed))
        {
            if (grounded || grounded2)
            {
                jumpUp = true;
            }
        }

    }
}