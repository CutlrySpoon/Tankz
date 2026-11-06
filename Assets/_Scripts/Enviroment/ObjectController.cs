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
    public Vector3 center => colliderTransform.position;

    private PlaceAction placeAction;
    private ObjectData objectData;

    bool isPlaced = false;

    public void SetObject(ObjectData data)
    {
        objectData = data;
        size = data.objectSize;
        spriteRenderer.sprite = data.objectSprite;
        spriteRenderer.color = data.useCustomColor ? data.customColor : gm.turnCount % 2 == 0 ? gm.teamAColour : gm.teamBColour;

        placeAction = data.placeAction;

        colliderTransform.localScale = new Vector3(data.objectSize.x, data.objectSize.y, 1f);

        int spriteSize = data.objectSize.x > data.objectSize.y ? (int)data.objectSize.x : (int)data.objectSize.y;
        spriteRenderer.transform.localScale = new Vector3(spriteSize, spriteSize, 1f);
    }

    public void PlaceObject()
    {
        isPlaced = true;

        colliderTransform.tag = "Object";
        gm.PurchaseObject(objectData.objectCost);

        placeAction?.OnPlace(this);
    }

    private void OnDestroy()
    {
        if (isPlaced)
            placeAction?.OnObjectDestroyed(this);
    }
}
