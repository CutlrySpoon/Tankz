using UnityEngine;
using System.Collections.Generic;

// This script is attached to a tile object. This allows for setting references and properties for the tile object.

// By OS.

public class TileObject : MonoBehaviour
{
    [SerializeField]
    public List<TileType> tileType;

    [System.Serializable]
    public class TileType
    {
        [SerializeField]
        public string name;

        [SerializeField]
        public Color color;
    }


    public Transform tileMesh { get; private set; }
    public Collider tileCollider { get; private set; }


    //Assign references to the tile mesh and collider to be used by manager.
    private void Awake()
    {
        tileMesh = GetComponentInChildren<MeshRenderer>().transform;
        tileCollider = GetComponentInChildren<Collider>();

        MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();
        meshRenderer.material.color = tileType[0].color;
    }
}
