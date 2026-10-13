using UnityEngine;

// ####TEMP#### This script is a temporary button script for testing the tile map generation.
// This will be replaced with a more advanced UI system in the future.

// By OS.

public class TestButton : MonoBehaviour
{
    public TileMapManager tileMapManager;

    public void OnButtonClick()
    {
        if (tileMapManager != null)
        {
            tileMapManager.GenerateNewMap();
        }

        else
        {
            Debug.LogWarning("TileMapManager reference is not set in TestButton.");
        }
    }
}
