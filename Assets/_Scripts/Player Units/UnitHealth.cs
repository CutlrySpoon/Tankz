using System.Collections;
using UnityEngine;
using static GameManager;
public class UnitHealth : MonoBehaviour
{
    public float health = 100;
    private float maxHealth;
    public float explosionResistance = 0f; // Resistance to explosive damage 0 = no resistance and 1 = full resistance
    public float flashTime = 0.5f; // Duration of the flash
    public GameObject deathFX; // Death effect prefab
    public AudioClip deathSFX;

    private MeshRenderer savedRenderer;
    private Color originalColor;
    private Coroutine flashCoroutine;

    private bool isActive = false;
    
    [SerializeField] private HealthBarPrefab healthUI; 

    public void SetUnit(int health, int explosionResistance, float offset)
    {
        // Initialize the unit's health and explosion resistance
        this.health = health;
        this.explosionResistance = explosionResistance;
        
        maxHealth = health;

        savedRenderer = GetComponent<MeshRenderer>();

        if (savedRenderer != null)
        {
            originalColor = savedRenderer.material.color;
        }

        if (healthUI != null)
            healthUI.gameObject.SetActive(false);

        SetHealthUI(offset);
    }

    public void SetHealthUI(float offset)
    {
        if (healthUI != null)
        {
            
            Vector3 pos = new Vector3(transform.position.x, transform.position.y - (offset / 2), transform.position.z);

            this.healthUI = Instantiate(healthUI, pos, Quaternion.identity, transform);

            healthUI.damageText.alpha = 1f;
            healthUI.healthBar.fillAmount = health / maxHealth;
            healthUI.healthBarText.text = health.ToString();
        }
    }

    public void SetFX(GameObject deathFX)
    {
        this.deathFX = deathFX;
    }

    public void SetHealthBar(HealthBarPrefab healthBar)
    {
        this.healthUI = healthBar;
    }

    private void Update()
    {
        if (healthUI != null)
        {
            // fade out the damage text over time
            healthUI.damageText.alpha -= Time.deltaTime * 0.2f;
        }

    }

    public void TakeDamage(int damage, bool isExplosive)
    {
        // Apply damage to the unit's health
        if (isExplosive)
        {
            // if explosive damage then apply resistance
            health -= damage * (1f - explosionResistance);
            if (healthUI != null)
            {
                healthUI.damageText.text = ("-" + (damage * (1f - explosionResistance)).ToString());
            }
        }
        else
        {
            // do damage normally
            health -= damage;
            if (healthUI != null)
            {
                healthUI.damageText.text = ("-" + damage.ToString());
            }
        }
        if (healthUI != null)
        {
            if (!isActive)
            {
                healthUI.gameObject.SetActive(true);
                isActive = true;
            }
            // Update the health bar and damage text
            healthUI.damageText.alpha = 1f;
            healthUI.damageText.fontSize = Mathf.Lerp(healthUI.damageText.fontSize, 5f, Time.deltaTime * 0.5f);

            healthUI.healthBar.fillAmount = health / maxHealth;

            healthUI.healthBarText.text = health.ToString();
        }

        // Trigger flash effect
        if (savedRenderer != null)
        {
            if (flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
            }
            flashCoroutine = StartCoroutine(FlashWhiteRoutine());
        }



        if (health <= 0)
        {
            if(isExplosive == false)
            {
                // Trigger death effect and sound
                if (deathFX != null)
                    Instantiate(deathFX, transform.position, Quaternion.identity);

                if (deathSFX != null)
                {
                    gm.audioSource.PlayOneShot(deathSFX, 0.1f);
                }
            }
            

            Destroy(gameObject);
        }
    }

    private IEnumerator FlashWhiteRoutine()
    {
        // Change the material colour to white and wait for the flash duration then reset
        savedRenderer.material.color = Color.white;
        Debug.Log("Flashing white");
        yield return new WaitForSeconds(flashTime);
        savedRenderer.material.color = originalColor;
        flashCoroutine = null;
    }
}