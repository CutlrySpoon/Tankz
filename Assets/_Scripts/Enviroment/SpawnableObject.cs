using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObject/PlaceableObject", order = 1)]
public class SpawnableObject : ScriptableObject
{
    [Header("Settings")]
    [SerializeField]
    private int health;
    [SerializeField]
    private bool isTiled;

    [Header("Graphics")]
    public Sprite objectSprite;
    public string objectName;
    [TextArea(2, 4)]
    public string objectDescription;
    public int objectCost;
    [Space(5)]

    [SerializeField]
    private Vector2 objectSize;
    [Space(10)]

    [Header("References")]
    [SerializeField]
    private TileObject tileObject;

    [SerializeField]
    private Material customMaterial;
}
