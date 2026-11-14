using UnityEngine;

// This is a scriptable object that is used to create new placeable objects in the game.

// By OS.

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObject/PlaceableObject", order = 1)]
public class ObjectData : ScriptableObject
{
    [Header("Settings")]
    public int health;
    public int explosionResistance;
    public int objectCost;
    public int maxObjects;
    [Space(5)]

    public bool requiresPower;
    public bool requiresResearch;
    [Space(5)]

    public bool isWall; // Can be placed anywhere but enemy territory
    public bool isBuilding; // Cannot be placed ontop another building
    public bool canRotate; // Enables rotation in 90 degree increments with q/e
    [Space(10)]

    public Vector2 objectSize;

    [Header("Graphics")]
    public Sprite objectSprite;
    public string objectName;
    [TextArea(2, 4)]
    public string objectDescription;
    [Space(5)]

    public bool useCustomColor; // Overrides team colours
    public Color customColor;
    [Space(10)]

    public GameObject deathEffect; // Spawns VFX on destroy
    [Space(5)]

    public PlaceAction placeAction; // Custom funciton to run on place and destroy

    public HealthBarPrefab healthBar; // Reference to health bar prefab
}
