using UnityEngine;
using UnityEngine.InputSystem;

public class UnitMovement : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    public LayerMask layermask;
<<<<<<< Updated upstream
=======
    public float maxFallSpeed = 15f;
    private Vector2 moveInput;
    private float verticalVelocity;
    private bool grounded;
    private float gravity = 9.81f; 
    public float jumpHeight = 8f;
    private bool jumpUp;
    

>>>>>>> Stashed changes
    void Awake()
    {
        layermask = LayerMask.GetMask("Ground");
    }

<<<<<<< Updated upstream
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
=======
    void FixedUpdate()
>>>>>>> Stashed changes
    {
        //check if the player is grounded by casting a ray down
        RaycastHit hit;
<<<<<<< Updated upstream

        if(!Physics.Raycast(transform.position, -transform.up, out hit, Mathf.Infinity, layermask))
        {
            target.position = target.position - 0.5f * Vector3.up;

        }
=======
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
>>>>>>> Stashed changes
    }

    public void Move(InputAction.CallbackContext context)
    {
<<<<<<< Updated upstream
        
        Vector2 input = context.ReadValue<Vector2>();
        Vector3 movement = new Vector3(input.x, input.y, 0f) * speed * Time.fixedDeltaTime;
        transform.Translate(movement, Space.World);
        
=======
        Vector2 input = context.ReadValue<Vector2>();
        moveInput = new Vector2(input.x, 0f);
        if (input.y > 0 && (context.started || context.performed))
        {
            if (grounded)
            {
                jumpUp = true;
            }
        }
>>>>>>> Stashed changes
    }
}