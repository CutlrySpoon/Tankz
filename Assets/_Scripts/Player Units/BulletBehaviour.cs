using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float startSpeed = 20f;
    public int damage = 10;
    public bool isExplosive = false;
    public float explosionRadius = 0f;
    public float lifetime = 5f;
    private float gravity = 9.81f;
    public float weight = 1f;
    private float shotStregth = 1f;
    private Vector3 velocity;
    public bool zigZag = false;
    public float zigZagfrequency = 5f;
    private Vector3 baseDirection;

    void Start()
    {
        //set the initial velocity of the bullet
        velocity = transform.right * startSpeed * shotStregth;
        baseDirection = transform.right;
    }

    void FixedUpdate()
    {
        // decrease lifetime and destroy the bullet if it has expired
        if (lifetime > 0f)
        {
            lifetime -= Time.fixedDeltaTime;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        if (zigZag)
        {
            // rotate the bullet's velocity vector by 45 degrees to create a zig-zag effect
            float flipDirection = Mathf.Sign(Mathf.Sin(Time.fixedDeltaTime * zigZagfrequency));
            Quaternion rotationOffset = Quaternion.Euler(0, 0, flipDirection * 45);
            Vector3 travelDir = rotationOffset * baseDirection;
            velocity.x = travelDir.x * startSpeed * shotStregth;
            velocity.y = travelDir.y * startSpeed * shotStregth;
        }
        else
        {
            velocity = baseDirection * startSpeed * shotStregth;
        }
        // apply gravity to the bullet's velocity and update its position
        velocity.y -= gravity * weight * Time.fixedDeltaTime;
        transform.position += velocity * Time.fixedDeltaTime;
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

        if(explosionRadius > 0f)
        {
            Vector3 center = transform.position;
            Collider[] colliders = Physics.OverlapSphere(center, explosionRadius);
            foreach (Collider hit in colliders)
            {
                if (hit.gameObject.transform.parent != null)
                {
                    Destroy(hit.gameObject.transform.parent.gameObject);
                }
            }
        }
        else if (collision.gameObject.transform.parent != null)
        {
            Destroy(collision.gameObject.transform.parent.gameObject);
        }


        Destroy(gameObject);

        if(collision.gameObject.GetComponent<UnitHealth>() != null)
        {
            UnitHealth doDamage = collision.gameObject.GetComponent<UnitHealth>();
            doDamage.TakeDamage(damage, isExplosive);

        }
    }




}