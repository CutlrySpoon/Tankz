using UnityEngine;

public class UnitHealth : MonoBehaviour
{

    public int health;
    public int  explosionResistane = 1;
    public float flashTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = 100;
    }

    public void TakeDamage(int damage, bool isExplosive)
    {
        bool damageTaken = false;
        if(isExplosive == true)
        {
            health = health - (damage / (explosionResistane));
            damageTaken = true;
        }
        else
        {
            health = health - damage;
            damageTaken = true;
        }
        
        if (damageTaken == true)
        {
           // MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
            //Color defaultColor = renderer.material.color;
            //
             //   renderer.material.color = Color.white;
              //  Debug.Log("flashing");

        }


        if (health <= 0)
        {
            // Add death sequence
            Destroy(this.gameObject);
        }
    }
}
