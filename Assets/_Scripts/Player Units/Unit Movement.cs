using UnityEngine;
using UnityEngine.InputSystem;

public class UnitMovement : MonoBehaviour
{
    public float speed = 5f;
    public LayerMask layermask;

    public float maxFallSpeed = 15f;
    private Vector2 moveInput;
    private float verticalVelocity;
    private bool grounded;
    private float gravity = 9.81f; 
    public float jumpHeight = 8f;
    private bool jumpUp;
    


    void Awake()
    {
        layermask = LayerMask.GetMask("Ground");
    }
    void FixedUpdate()

    {
        //check if the player is grounded by casting a ray down
        RaycastHit hit;


        if(!Physics.Raycast(transform.position, -transform.up, out hit, Mathf.Infinity, layermask))
        {
            transform.position = transform.position - 0.5f * Vector3.up;

        }

        grounded = Physics.Raycast(transform.position, Vector3.down, out hit, 0.55f, layermask);

        if (grounded)
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

        if (!grounded)
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
            if (grounded)
            {
                jumpUp = true;
            }
        }

    }
}