using UnityEngine;

// This is a scriptable object that is used to create new placeable objects in the game.

// By OS.

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObject/PlaceableObject", order = 1)]
public class ObjectData : ScriptableObject
{
    [Header("Settings")]
    public int health;
    public int objectCost;
    public int maxObjects;
    [Space(5)]

    public bool requiresPower;
    public bool requiresResearch;
    [Space(5)]

    public bool isWall;
    public bool isBuilding;
    public bool canRotate;
    [Space(10)]

    public Vector2 objectSize;

    [Header("Graphics")]
    public Sprite objectSprite;
    public string objectName;
    [TextArea(2, 4)]
    public string objectDescription;
    [Space(5)]

    public bool useCustomColor;
    public Color customColor;
    [Space(10)]

    public PlaceAction placeAction;
}
