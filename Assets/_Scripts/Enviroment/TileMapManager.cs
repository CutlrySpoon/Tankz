using UnityEngine;
using System.Collections.Generic;

// This is a manager script. This controlls the procedrual, seed based generation of the tile map with perlin noise.

// By OS.

public class TileMapManager : MonoBehaviour
{
    // References and settings for tile map generation. Customise in the inspector.
    [SerializeField]
    private GameObject tilePrefab;

    [SerializeField]
    private int mapWidth = 10;

    [SerializeField]
    private int mapHeight = 10;
    [Space(20)]

    [SerializeField]
    [Range(0.1f, 1f)]
    private float mapResolution = 1f;   // Sets the size of each tile.

    [SerializeField]
    [Range(0.25f, 0.75f)]
    private float noiseVolume = 0.5f;   // Sets the sensitivity of the noise gradient for tile generation.

    [SerializeField]
    [Range(1f, 15f)]
    private float noiseScale = 1f;      // Sets the size of the noise pattern.

    [SerializeField]
    [Range(1, 10000)]
    private int seed = 1;               // Adds this value to noise coordinates for unique generation.


    private bool isMapGenerated = false;

    private float xPos;
    private float yPos;

    private Camera camera;

    private List<GameObject> tileGrid = new List<GameObject>();


    // ####TEMP#### assigns camera for centering.
    private void Awake()
    {
        camera = Camera.main.GetComponent<Camera>();
    }

    private void Update()
    {
        if (!isMapGenerated)
            GenerateTileGrid();
    }

    // Generates the tile grid based on settings.
    private void GenerateTileGrid()
    {
        while (xPos < mapWidth * mapResolution)
        {
            while (yPos < mapHeight * mapResolution)
            {
                float noiseValue = GenerateNoise((xPos / mapResolution), (yPos / mapResolution));

                if (CanGenerateTile(noiseValue))
                {
                    // Generates tile object
                    GameObject tile = Instantiate(tilePrefab, new Vector3(xPos, yPos, 0), Quaternion.identity, this.transform);
                    tileGrid.Add(tile);

                    // Sets the scale of the tile mesh and collider based on map resolution.
                    // (The mesh is scaled to 90% of the map resolution to allow for a small gap between tiles.)
                    float tileResolution = (mapResolution * 0.9f) / 10;
                    tile.GetComponent<TileObject>().tileMesh.localScale = new Vector3(tileResolution, tileResolution, tileResolution);
                    tile.GetComponent<TileObject>().tileCollider.transform.localScale = new Vector3(mapResolution, mapResolution, mapResolution);
                }

                yPos += mapResolution;
            }

            xPos += mapResolution;

            yPos = 0;
        }

        isMapGenerated = true;

        //####TEMP#### Centers camera on the generated tile map.
        camera.GetComponent<CameraController>().CenterCamera(mapWidth, mapHeight, mapResolution);
    }

    // Generates perlin noise value based on x / y position of current tile.
    // Returns a float value between 0 and 1.
    private float GenerateNoise(float x, float y)
    {
        float xCoord = (x + seed) / (mapHeight + mapWidth) * noiseScale;
        float yCoord = (y + seed) / (mapHeight + mapWidth) * noiseScale;

        float sample = Mathf.PerlinNoise(xCoord, yCoord);

        return sample;
    }

    // Checks if the generated noise value is above the sensitivity threshold to determine if a tile should be generated.
    // Higher volume = more tiles generated, lower volume = less tiles generated.
    private bool CanGenerateTile(float value)
    {
        if (value <= noiseVolume)
        {
            return true;
        }

        return false;
    }

    private void RandomiseSeed()
    {
        seed = Random.Range(1, 10000);
    }

    // Public function to destroy current tiles and generate a new map.
    public void GenerateNewMap()
    {
        foreach (GameObject tile in tileGrid)
        {
            Destroy(tile);
        }

        tileGrid.Clear();

        xPos = 0;
        yPos = 0;
        
        RandomiseSeed();
        camera.GetComponent<CameraController>().CenterCamera(mapWidth, mapHeight, mapResolution);

        isMapGenerated = false;
    }
}
