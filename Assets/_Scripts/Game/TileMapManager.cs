using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

// This is a singleton manager script. This controlls the procedrual, seed based generation of the tile map with perlin noise.

// By OS.

public class TileMapManager : MonoBehaviour
{
    private static TileMapManager instance;

    // Check if instance exists already.
    public static TileMapManager tm
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("TileMapManager");
                instance = obj.AddComponent<TileMapManager>();
            }

            return instance;
        }
    }

    // References and settings for tile map generation. Customise in the inspector.
    [SerializeField]
    private GameObject tilePrefab;

    [SerializeField]
    public int mapWidth = 150;

    [SerializeField]
    public int mapHeight = 75;
    [Space(20)]

    [SerializeField]
    [Range(0.35f, 0.55f)]
    private float noiseVolume = 0.5f;   // Sets the sensitivity of the noise gradient for tile generation.
    public float minNoiseVolume { get { return 0.35f; } }
    public float maxNoiseVolume { get { return 0.55f; } }

    [SerializeField]
    [Range(8f, 14f)]
    private float noiseScale = 12f;      // Sets the size of the noise pattern.
    public float minNoiseScale { get { return 8f; } }
    public float maxNoiseScale { get { return 14f; } }

    [SerializeField]
    private int seed = 1;               // Adds this value to noise coordinates for unique generation.


    private List<GameObject> tileGrid = new List<GameObject>();

    private bool isMapGenerated = true;

    private float xPos;
    private float yPos;


    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (!isMapGenerated)
            GenerateTileGrid();
    }

    // Public function to generate map with outside values.
    public void SetMap(float volumeValue, float scaleValue, int seedValue, bool useNewValue)
    {
        if (useNewValue)
        {
            noiseVolume = volumeValue;
            noiseScale = scaleValue;
            seed = seedValue;
        }


        if (tileGrid.Count != 0)
        {
            foreach (GameObject tile in tileGrid)
            {
                Destroy(tile);
            }

            tileGrid.Clear();
        }

        xPos = 0;
        yPos = 0;

        isMapGenerated = false;
    }

    // Generates the tile grid based on settings.
    private void GenerateTileGrid()
    {
        while (xPos < mapWidth)
        {
            while (yPos < mapHeight)
            {
                float noiseValue = GenerateNoise((xPos), (yPos));

                if (CanGenerateTile(noiseValue))
                {
                    // Generates tile object
                    GameObject tile = Instantiate(tilePrefab, new Vector3(xPos, yPos, 0), Quaternion.identity, this.transform);
                    tileGrid.Add(tile);
                }

                ++yPos;
            }

            ++xPos;

            yPos = 0;
        }

        isMapGenerated = true;
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


    //####TEMP#### Generates a new map for testing only.
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
        
        isMapGenerated = false;
    }

    //####TEMP####
    private void RandomiseSeed()
    {
        seed = Random.Range(1, 10000);
    }
}
