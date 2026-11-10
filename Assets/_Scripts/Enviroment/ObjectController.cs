using Unity.VisualScripting;
using UnityEngine;
using static GameManager;

public class ObjectController : MonoBehaviour
{
    // This is the main controller for player spawned objects.

    // By OS.

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Transform colliderTransform;

    public Vector2 size { get; private set; }

    private UnitHealth unitHealth;

    public Vector2Int WorldSize
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

    public void SetObject(ObjectData data, ResourceUI resourceUI)
    {
        objectData = data;

        unitHealth = this.AddComponent<UnitHealth>();
        unitHealth.SetUnit(data.health, data.explosionResistance);

        if (data.deathEffect != null)
            unitHealth.SetFX(data.deathEffect);

        ui = resourceUI;
        size = data.objectSize;
        spriteRenderer.sprite = data.objectSprite;
        spriteRenderer.color = data.useCustomColor ? data.customColor : gm.turnCount % 2 == 0 ? gm.teamAColour : gm.teamBColour;

        placeAction = data.placeAction;

        colliderTransform.localScale = new Vector3(data.objectSize.x, data.objectSize.y, 1f);
    }

    public void PlaceObject()
    {
        isPlaced = true;

        if (!objectData.isBuilding)
            colliderTransform.tag = "Object";
        else
            colliderTransform.tag = "Building";
        
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
            

        placeAction?.OnPlace(this);

        ui.PlaceObject();
    }


    private void OnDestroy()
    {
        if (isPlaced)
            placeAction?.OnObjectDestroyed(this);
    }
}
