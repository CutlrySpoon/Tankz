using UnityEngine;

// ####TEMP#### This script is a temporary camera controller for centering the camera on the tile map.
// This will be replaced with a more advanced camera controller in the future.

// By OS.

public class CameraController : MonoBehaviour
{
    public void CenterCamera(int width, int height, float resolution)
    {
        float halfWidth = (width * resolution) / 2f;
        float halfHeight = (height * resolution) / 2f;

        transform.position = new Vector3(halfWidth, halfHeight, transform.position.z);
    }
}
