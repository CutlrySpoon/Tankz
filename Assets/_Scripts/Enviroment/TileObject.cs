using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Numerics;
using static GameManager;

// This script is attached to a tile object. This allows for setting references and properties for the tile object.

// By OS.

public class TileObject : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer meshRenderer;


    public void SetMaterial(Material mat, bool isTeamColour)
    {
        if (isTeamColour)
        {
            if (gm.turnCount % 2 == 0)
            {
                meshRenderer.material = gm.teamBMaterial;
            }

            else
            {
                meshRenderer.material = gm.teamBMaterial;
            }
        }

        else
        {
            meshRenderer.material = mat;
        }
        
    }
}
