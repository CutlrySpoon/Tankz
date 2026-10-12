using UnityEngine;
using System.Collections.Generic;

public class TileMapManager : MonoBehaviour
{
    [SerializeField]
    private GameObject tilePrefab;

    [SerializeField]
    private float mapWidth = 10;

    [SerializeField]
    private float mapHeight = 10;

    [SerializeField]
    [Range(0.1f, 1f)]
    public float mapResolution = 1f;


    private float xPos;
    private float yPos;

    private void Awake()
    {
        mapWidth *= mapResolution;
        mapHeight *= mapResolution;
    }

    private void Update()
    {
        while(xPos < mapWidth)
        {
            while (yPos < mapHeight)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(xPos, yPos, 0), Quaternion.identity, this.transform);

                float tileResolution = mapResolution * 0.9f;
                tile.GetComponent<TileObject>().tileMesh.localScale = new Vector3(tileResolution, tileResolution, tileResolution);
                tile.GetComponent<TileObject>().tileCollider.transform.localScale = new Vector3(mapResolution, mapResolution, mapResolution);

                yPos += mapResolution;               
            }

            xPos += mapResolution;

            yPos = 0;
        }
    }
}
