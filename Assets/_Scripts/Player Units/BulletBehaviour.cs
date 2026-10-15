using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float startSpeed = 10f;
    public float maxSpeed = 20f;
    public float weight = 1f;
    public float damage = 10f;
    public float lifetime = 10f;
    private float currentSpeed;
    private Vector3 lastPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Awake()
    {
        currentSpeed = startSpeed;
        lastPosition = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        lastPosition = transform.position;
        if (lifetime > 0f)
        {
            lifetime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }

        
        if(lastPosition.y > transform.position.y && currentSpeed < maxSpeed)
        {
            currentSpeed += weight * Time.deltaTime;
        }
        else
        {
            currentSpeed -= weight * Time.deltaTime;
        }

        Mathf.Clamp(currentSpeed, 1.2f, maxSpeed);
        transform.Translate(Vector3.up * currentSpeed * Time.deltaTime);
        lastPosition = transform.position;
    }
}
