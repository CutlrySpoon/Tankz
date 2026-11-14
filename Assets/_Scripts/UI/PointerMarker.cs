using UnityEngine;

// Reference to pointer UI prefab components and setter

public class PointerMarker : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public void SetMarker(Sprite sprite, Color color)
    {
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = color;
    }
}
