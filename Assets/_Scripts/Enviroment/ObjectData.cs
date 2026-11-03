using UnityEngine;

// This is a scriptable object that is used to create new placeable objects in the game.

// By OS.

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObject/PlaceableObject", order = 1)]
public class ObjectData : ScriptableObject
{
    [Header("Settings")]
    public int health;
    public int objectCost;
    [Space(5)]

    public Vector2 objectSize;

    [Header("Graphics")]
    public Sprite objectSprite;
    public string objectName;
    [TextArea(2, 4)]
    public string objectDescription;
    [Space(10)]

    public bool useCustomColor;
    public Color customColor;
}
