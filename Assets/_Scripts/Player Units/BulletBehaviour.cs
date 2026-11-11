using UnityEditor;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float startSpeed = 20f;
    public int damage = 10;
    public int penetration = 1;
    private bool isExplosive = false;
    public float explosionRadius = 0f;
    public float lifetime = 5f;
    private float gravity = 9.81f;
    public float weight = 1f;
    public float shotStregth = 1f;
    public GameObject explosionEffectPrefab;
    private Vector3 velocity;
    public float zigZagfrequency = 5f;
    private Vector3 baseDirection;
    private Vector3 collisionPoint;
    private bool firstCollision = true;
    private bool detonate = false;
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
        if (penetration != 0 && Vector3.Distance(collisionPoint, transform.position) > penetration && firstCollision == false)
        {
            Destroy(gameObject);
        }
        Vector3 travelDir = baseDirection;
        if (zigZagfrequency > 0)
        {
            // rotate the bullet's velocity vector by 45 degrees to create a zig-zag effect
            float flipDirection = Mathf.Sign(Mathf.Sin(Time.time * zigZagfrequency));
            Quaternion rotationOffset = Quaternion.Euler(0, 0, flipDirection * 45f);
            travelDir = rotationOffset * baseDirection;
        }

        // apply gravity to the bullet's velocity and update its position
        velocity.x = travelDir.x * startSpeed * shotStregth;
        velocity.y -= gravity * weight * Time.fixedDeltaTime;
        transform.position += velocity * Time.fixedDeltaTime;
        if (velocity != Vector3.zero)
        {
            // rotate the bullet to face the direction of its velocity
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
        if (penetration <= 0 && explosionRadius <= 0)
        {
            Destroy(gameObject);
        }
       
    }


    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.layer == LayerMask.NameToLayer("Units") || collision.gameObject.tag == ("Building") || collision.gameObject.tag == ("Object"))
        {
            detonate = true;
        }
        
        //after first collision, reduce penetration and continue moving unitl it reaches 0
        if (firstCollision == true)
        {
            firstCollision = false;
            collisionPoint = transform.position;

        }
        // logic for doing damage
        if (explosionRadius > 0f && Vector3.Distance(collisionPoint, transform.position) >= penetration || detonate)
        {
            
            isExplosive = true;
            Vector3 center = transform.position;
            Collider[] colliders = Physics.OverlapSphere(center, explosionRadius);
            
            foreach (Collider hit in colliders)
            {
                if (hit.gameObject.transform.GetComponent<UnitHealth>() != null)
                {
                    //Destroy(hit.gameObject.transform.parent.gameObject);
                    Debug.Log("Hit " + hit.gameObject.name + " with explosion");
                    hit.gameObject.GetComponent<UnitHealth>().TakeDamage(damage, isExplosive);
                }

                if (hit.gameObject.transform.GetComponentInParent<UnitHealth>() != null)
                {
                    //Destroy(hit.gameObject.transform.parent.gameObject);
                    Debug.Log("Hit " + hit.gameObject.name + " with explosion");
                    hit.gameObject.GetComponentInParent<UnitHealth>().TakeDamage(damage, isExplosive);
                }
            }
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (collision.gameObject.transform.GetComponent<UnitHealth>() != null)
        {
            collision.gameObject.transform.gameObject.GetComponent<UnitHealth>().TakeDamage(damage, isExplosive);
        }

        
        

       
    }




}