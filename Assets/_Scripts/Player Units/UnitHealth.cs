using System.Collections;
using UnityEngine;

public class UnitHealth : MonoBehaviour
{
    public float health = 100;
    public float explosionResistance = 0f;
    public float flashTime = 0.5f; // Duration of the flash

    private MeshRenderer savedRenderer;
    private Color originalColor;
    private Coroutine flashCoroutine;

    void Start()
    {
        savedRenderer = GetComponent<MeshRenderer>();
        if (savedRenderer != null)
        {
            originalColor = savedRenderer.material.color;
        }
    }

    public void TakeDamage(int damage, bool isExplosive)
    {
        if (isExplosive)
        {
            health -= damage * (1f - explosionResistance);
        }
        else
        {
            health -= damage;
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