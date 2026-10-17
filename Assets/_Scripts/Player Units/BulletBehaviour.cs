using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float startSpeed = 20f;
    public float damage = 10f;
    public float lifetime = 5f;
    private float gravity = 9.81f;
    public float weight = 1f;
    private float shotStregth = 1f;
    public LineRenderer shotAssistLinePrefab;
    private Vector3 velocity;

    void Start()
    {
        //set the initial velocity of the bullet
        velocity = transform.right * startSpeed * shotStregth;
    }

    void Update()
    {
        // decrease lifetime and destroy the bullet if it has expired
        if (lifetime > 0f)
        {
            lifetime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
            return;
        }


        // apply gravity to the bullet's velocity and update its position
        velocity.y -= gravity * weight * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
        if (velocity != Vector3.zero)
        {
            // rotate the bullet to face the direction of its velocity
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }

    }


    private void OnCollisionEnter(Collision collision)
    {

        // add logic for destroying tiles and doing damage

        Destroy(collision.gameObject);
        Destroy(gameObject);
    }




}