using UnityEngine;
using UnityEngine.InputSystem;

public class UnitMovement : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    public LayerMask layermask;
    public float maxFallSpeed = 4f;
    private Vector3 movement;
    private float fallSpeed;
    private bool grounded;
    void Awake()
    {
        layermask = LayerMask.GetMask("Ground");
        target = transform;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;
        transform.Translate(movement, Space.World);
        if (!Physics.Raycast(transform.position, -transform.up, out hit, 0.55f, layermask))
        {
            grounded = false;
        }
        else
        {
            grounded = true;
        }
        if (!grounded)
        {
            fallSpeed = fallSpeed + 8.9f * Time.deltaTime;
            target.position = target.position + (Vector3.down * Mathf.Clamp(fallSpeed, 0, maxFallSpeed) * Time.deltaTime);
        }
        if (grounded)
        {
            fallSpeed = 0f;
        }
        
    }
    public void Move(InputAction.CallbackContext context)
    {

        Vector2 input = context.ReadValue<Vector2>();
        movement = new Vector3(input.x, input.y, 0f) * speed * Time.fixedDeltaTime;
        
        

    }
}
