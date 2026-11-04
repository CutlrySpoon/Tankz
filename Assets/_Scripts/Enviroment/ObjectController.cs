using UnityEngine;

public class ObjectController : MonoBehaviour
{
    // This is the main controller for player spawned objects.

    // By OS.

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Transform collider;

    public void SetObject(Sprite sprite, Color color, Vector2 size)
    {
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = color;

        collider.localScale = new Vector3(size.x, size.y, 1f);
    }

    public void PlaceObject()
    {
        collider.tag = "Object";
    }
}
