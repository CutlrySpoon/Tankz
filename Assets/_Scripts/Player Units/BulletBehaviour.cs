using System;
using UnityEditor;
using UnityEngine;
using static GameManager;

public class BulletBehaviour : MonoBehaviour
{
    [Header("Bullet stats")]
    [SerializeField] private float startSpeed = 20f;
    [SerializeField] private int damage = 10;
    [SerializeField] private int penetration = 1;
    private bool isExplosive = false;
    [SerializeField] private float explosionRadius = 0f;
    [SerializeField] private float lifetime = 5f;
    private float gravity = 9.81f;
    [SerializeField] private float weight = 1f;
    public float shotStregth = 1f;
    [SerializeField] private GameObject explosionEffectPrefab;
    private Vector3 velocity;
    [SerializeField] private float zigZagfrequency = 5f;
    private Vector3 baseDirection;
    private Vector3 collisionPoint;
    private bool firstCollision = true;
    private bool detonate = false;

    [Header("Audio")]
    [SerializeField]
    private AudioClip shootSound;
    [SerializeField]
    private AudioClip explosionSound;


    void Start()
    {
        //set the initial velocity of the bullet
        velocity = transform.right * startSpeed * shotStregth;
        baseDirection = transform.right;

        gm.audioSource.PlayOneShot(shootSound);
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
        // if the bullet has a penetration value and it has traveled further than that the penetration then destroy it
        if (penetration != 0 && Vector3.Distance(collisionPoint, transform.position) > penetration && firstCollision == false && explosionRadius <= 0)
        {
            Destroy(gameObject);
        }

        Vector3 travelDir = baseDirection;
        if (zigZagfrequency > 0)
        {
            // Rotate the bullet's direction vector by 45 degrees back and forth
            float flipDirection = Mathf.Sign(Mathf.Sin(Time.fixedTime * zigZagfrequency));
            Quaternion rotationOffset = Quaternion.Euler(0, 0, flipDirection * 45f);
            travelDir = rotationOffset * baseDirection;
        }

        // Apply travel direction speed and gravity to velocity
        velocity.x = travelDir.x * startSpeed * shotStregth;

        //if is not zigzag then apply gravity
        if (zigZagfrequency == 0)
            velocity.y -= (gravity * weight * Time.fixedDeltaTime);
        else // if is zigzag then apply gravity to the y velocity based on the travel direction
            velocity.y = (travelDir.y * startSpeed * shotStregth) - (gravity * weight * Time.fixedDeltaTime);

        transform.position += velocity * Time.fixedDeltaTime;

        if (velocity != Vector3.zero)
        {
            // Rotate the bullet to face the direction of its velocity
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
        //Destroy if has no penetration or explosion
        if (penetration <= 0 && explosionRadius <= 0)
        {
            Destroy(gameObject);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        // if has hit an object other than a tile detonate the charge if it is explosive
        if ((collision.gameObject.layer == LayerMask.NameToLayer("Units") || collision.gameObject.tag == ("Building") || collision.gameObject.tag == ("Object")) && explosionRadius > 0)
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
                // apply damage to component
                if (hit.gameObject.transform.GetComponent<UnitHealth>() != null)
                {
                    Debug.Log("Hit " + hit.gameObject.name + " with explosion");
                    hit.gameObject.GetComponent<UnitHealth>().TakeDamage(damage, isExplosive);
                }
                // apply damage to component in the parent
                if (hit.gameObject.transform.GetComponentInParent<UnitHealth>() != null)
                {
                    Debug.Log("Hit " + hit.gameObject.name + " with explosion");
                    hit.gameObject.GetComponentInParent<UnitHealth>().TakeDamage(damage, isExplosive);
                }
            }
            // spawn the explosion fx
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            gm.audioSource.PlayOneShot(explosionSound);

            Destroy(gameObject);
        }
        // if not explosive then apply regular damage
        else if (collision.gameObject.transform.GetComponent<UnitHealth>() != null)
        {
            collision.gameObject.transform.gameObject.GetComponent<UnitHealth>().TakeDamage(damage, isExplosive);
            if(collision.gameObject.layer == LayerMask.NameToLayer("Units") || collision.gameObject.tag == ("Building") || collision.gameObject.tag == ("Object"))
            {
                Destroy(gameObject);
            }
        }
        // if not explosive then apply regular damage to parent
        else if (collision.gameObject.transform.GetComponentInParent<UnitHealth>() != null)
        {
            collision.gameObject.transform.gameObject.GetComponentInParent<UnitHealth>().TakeDamage(damage, isExplosive);
            if (collision.gameObject.layer == LayerMask.NameToLayer("Units") || collision.gameObject.tag == ("Building") || collision.gameObject.tag == ("Object"))
            {
                Destroy(gameObject);
            }
        }
    }
}