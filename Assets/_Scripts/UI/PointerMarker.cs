using UnityEngine;

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
