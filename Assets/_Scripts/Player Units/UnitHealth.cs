using System;
using System.Collections;
using UnityEditor.Build;
using UnityEngine;
public class UnitHealth : MonoBehaviour
{
    public float health = 100;
    private float maxHealth;
    public float explosionResistance = 0f;
    public float flashTime = 0.5f; // Duration of the flash
    public GameObject deathFX;

    private MeshRenderer savedRenderer;
    private Color originalColor;
    private Coroutine flashCoroutine;
    
    [SerializeField] private HealthBarPrefab healthUI;

    public void SetUnit(int health, int explosionResistance, float offset)
    {
        this.health = health;
        this.explosionResistance = explosionResistance;
        
        maxHealth = health;

        savedRenderer = GetComponent<MeshRenderer>();

        if (savedRenderer != null)
        {
            originalColor = savedRenderer.material.color;
        }

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
            healthUI.damageText.alpha -= Time.deltaTime * 0.2f;
        }

    }

    public void TakeDamage(int damage, bool isExplosive)
    {

        if (isExplosive)
        {
            health -= damage * (1f - explosionResistance);
            if (healthUI != null)
            {
                healthUI.damageText.text = ("-" + (damage * (1f - explosionResistance)).ToString());
            }
        }
        else
        {
            health -= damage;
            if (healthUI != null)
            {
                healthUI.damageText.text = ("-" + damage.ToString());
            }
        }
        if (healthUI != null)
        {
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
            if(deathFX != null && isExplosive == false)
            {
                Instantiate(deathFX, transform.position, Quaternion.identity);
            }   
            Destroy(gameObject);
        }
    }

    private IEnumerator FlashWhiteRoutine()
    {
        savedRenderer.material.color = Color.white;
        Debug.Log("Flashing white");
        yield return new WaitForSeconds(flashTime);
        savedRenderer.material.color = originalColor;
        flashCoroutine = null;
    }
}