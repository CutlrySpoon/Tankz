using UnityEngine;
using UnityEngine.InputSystem;

public class UnitMovement : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    public LayerMask layermask;
    void Awake()
    {
        layermask = LayerMask.GetMask("Ground");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if(!Physics.Raycast(transform.position, -transform.up, out hit, Mathf.Infinity, layermask))
        {
            target.position = target.position - 0.5f * Vector3.up;

        }
    }
    public void Move(InputAction.CallbackContext context)
    {
        
        Vector2 input = context.ReadValue<Vector2>();
        Vector3 movement = new Vector3(input.x, input.y, 0f) * speed * Time.fixedDeltaTime;
        transform.Translate(movement, Space.World);
        
    }
}
