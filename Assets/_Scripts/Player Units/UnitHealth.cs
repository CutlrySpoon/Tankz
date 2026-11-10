using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
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
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private Image healthBar;
    [SerializeField] private TMP_Text healthBarText;
    [SerializeField] private GameObject healthBarPrefab;

    void Start()
    {
        Instantiate(healthBarPrefab,transform.position, Quaternion.identity, transform);
        maxHealth = health;
        if(damageText != null)
        {
            damageText.alpha = 0f;
        }
        savedRenderer = GetComponent<MeshRenderer>();
        if (savedRenderer != null)
        {
            originalColor = savedRenderer.material.color;
        }
    }

    public void SetUnit(int health, int explosionResistance)
    {
        this.health = health;
        this.explosionResistance = explosionResistance;
    }

    public void SetFX(GameObject deathFX)
    {
        this.deathFX = deathFX;
    }

    private void Update()
    {
        if(damageText != null)
        {
            damageText.alpha -= Time.deltaTime * 0.2f;
        }

    }

    public void TakeDamage(int damage, bool isExplosive)
    {

        if (isExplosive)
        {
            health -= damage * (1f - explosionResistance);
            if (damageText != null)
            {
                damageText.text = ("-" + (damage * (1f - explosionResistance)).ToString());
            }
        }
        else
        {
            health -= damage;
            if (damageText != null)
            {
                damageText.text = ("-" + damage.ToString());
            }
        }
        if(damageText != null)
        {
            damageText.alpha = 1f;
            damageText.fontSize = Mathf.Lerp(damageText.fontSize, 5f, Time.deltaTime * 0.5f);
        }
        if (healthBar != null)
        {
            healthBar.fillAmount = health / maxHealth;
        }
        if (healthBarText != null)
        {
            healthBarText.text = health.ToString();
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