using System;
using UnityEngine;
using static GameManager;

// This manages all resources in the game, including buildings, units and walls. It also handles the UI for these resources.

// By OS.

public class ResourceManager : MonoBehaviour
{
    private static ResourceManager instance;

    // Check if instance exists already.
    public static ResourceManager rm
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("TileMapManager");
                instance = obj.AddComponent<ResourceManager>();
            }

            return instance;
        }
    }

    [Header("Resources")]
    [SerializeField]
    private ObjectData[] buildings;
    [SerializeField]
    private ObjectData[] units;
    [SerializeField]
    private ObjectData[] walls;
    [Space(10)]

    [Header("References")]
    [SerializeField]
    private PointerController pointerController;
    [SerializeField]
    private ObjectController objectController;
    [Space(10)]

    [Header("UI")]
    [SerializeField]
    private ResourceUI prefabUI;
    [SerializeField]
    private GameObject buildingsContainer;
    [SerializeField]
    private GameObject unitsContainer;
    [SerializeField]
    private GameObject wallsContainer;

    [SerializeField]
    private ResourceButton[] buttons;

    private void Awake()
    {
        instance = this;
    }

    // Populate the resource UI with all available buildings, units and walls.
    private void Start()
    {
        foreach (ObjectData building in buildings)
        {
            ResourceUI obj = Instantiate(prefabUI, buildingsContainer.transform);
            obj.SetUI(building);
        }

        foreach (ObjectData unit in units)
        {
            ResourceUI obj = Instantiate(prefabUI, unitsContainer.transform);
            obj.SetUI(unit);
        }

        foreach (ObjectData wall in walls)
        {
            ResourceUI obj = Instantiate(prefabUI, wallsContainer.transform);
            obj.SetUI(wall);
        }
    }

    // Spawn the selected object, set its data and start following the pointer.
    public void SpawnObject(ObjectData objectData, ResourceUI ui)
    {
        CloseMenu();

        pointerController.StopFollowPointer();

        Color objectColour;

        if (objectData.useCustomColor)
            objectColour = objectData.customColor;
        else
        {
            if (gm.turnCount % 2 == 0)
                objectColour = gm.teamAColour;
            else
                objectColour = gm.teamBColour;
        }

        ObjectController obj = Instantiate(this.objectController, pointerController.transform.position, Quaternion.identity);
        obj.SetObject(objectData, ui);

        pointerController.FollowPointer(obj.gameObject);
    }

    public void CloseMenu()
    {
        foreach (ResourceButton button in buttons)
        {
            button.CloseMenu();
        }
    }
}
