using Unity.VisualScripting;
using UnityEngine;
using static GameManager;

// This is the main controller for player spawned objects.

// By OS.

public class ObjectController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Transform colliderTransform;

    public Vector2 size { get; private set; }

    private UnitHealth unitHealth;

    public Vector2Int WorldSize // Returns the real world size and accomodates for rotation.
    {
        get
        {
            int quarterTurns = Mathf.RoundToInt(transform.eulerAngles.z / 90f);
            bool sideways = quarterTurns % 2 != 0;

            int width = Mathf.RoundToInt(size.x);
            int height = Mathf.RoundToInt(size.y);

            return sideways ? new Vector2Int(height, width) : new Vector2Int(width, height);
        }
    }

    public Vector3 center => colliderTransform.position;

    public ObjectData objectData { get; private set; }

    private PlaceAction placeAction;   

    bool isPlaced = false;

    ResourceUI ui;

    //Passes all data from object data onto the world space object and assignes references
    public void SetObject(ObjectData data, ResourceUI resourceUI)
    {
        objectData = data;

        unitHealth = this.AddComponent<UnitHealth>();

        if (data.healthBar != null)
            unitHealth.SetHealthBar(data.healthBar);

        if (data.deathEffect != null)
            unitHealth.SetFX(data.deathEffect);

        unitHealth.SetUnit(data.health, data.explosionResistance, data.objectSize.y);


        ui = resourceUI;
        size = data.objectSize;
        spriteRenderer.sprite = data.objectSprite;
        spriteRenderer.color = data.useCustomColor ? data.customColor : gm.turnCount % 2 == 0 ? gm.teamAColour : gm.teamBColour;

        placeAction = data.placeAction;

        colliderTransform.localScale = new Vector3(data.objectSize.x, data.objectSize.y, 1f);
    }

    // Function to run on object place
    public void PlaceObject()
    {
        isPlaced = true;

        // Set correct tag
        if (!objectData.isBuilding)
            colliderTransform.tag = "Object";
        else
            colliderTransform.tag = "Building";
        
        // Charge the correct team
        if (gm.turnCount % 2 == 0)
        {
            gm.teamACash -= objectData.objectCost;

            if (objectData.requiresResearch)
                gm.teamAResearch--;
            if (objectData.requiresPower)
                gm.teamAPower--;
        }
            
        else
        {
            gm.teamBCash -= objectData.objectCost;

            if (objectData.requiresResearch)
                gm.teamBResearch--;
            if (objectData.requiresPower)
                gm.teamBPower--;
        }

        // Dissable trigger to allow collision with object
        Collider col = colliderTransform.GetComponent<Collider>();
        col.isTrigger = false;
            
        // Run custom place action if present
        placeAction?.OnPlace(this);

        ui.PlaceObject();
    }

    // Run custom destroy funciton
    private void OnDestroy()
    {
        if (isPlaced)
            placeAction?.OnObjectDestroyed(this);
    }
}
