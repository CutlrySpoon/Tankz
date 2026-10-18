using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

// This is a camera controller which is used to drag and zoom the camera around the playable area.

// By OS.

public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private GameObject camParent;
    [Space(10)]


    [Header("Settings")]
    [SerializeField]
    private int zoomIncrement;

    [SerializeField]
    private int minZoom;

    [SerializeField]
    private int maxZoom;


    private Vector3 mousePos;
    private Vector3 dragOrigin;
    private Vector3 dragDifference;

    private Vector2 scroll;
    private int scrollCount = 0;

    private bool holdingDrag;


    private void Start()
    {
        cam.orthographicSize = maxZoom;
    }

    // Clamp the camera max position based on the map size and zoom level
    private void LateUpdate()
    {
        float minX = TileMapManager.Instance.mapWidth / 2 - scrollCount * (zoomIncrement * 1.6f);
        float maxX = TileMapManager.Instance.mapWidth / 2 + scrollCount * (zoomIncrement * 1.6f);

        float minY = TileMapManager.Instance.mapHeight / 2 - scrollCount * (zoomIncrement * 0.8f);
        float maxY = TileMapManager.Instance.mapHeight / 2 + scrollCount * (zoomIncrement * 0.8f);

        Vector3 clampPos = camParent.transform.position;

        clampPos.x = Mathf.Clamp(clampPos.x, minX, maxX);
        clampPos.y = Mathf.Clamp(clampPos.y, minY, maxY);

        camParent.transform.position = clampPos;        
    }

    // Dragging and scrolling
    private void Update()
    {
        if (holdingDrag)
        {
            dragDifference = cam.ScreenToWorldPoint(mousePos) - camParent.transform.position;
            camParent.transform.position = dragOrigin - dragDifference;
        }

        // Scroll Up
        if (scroll.y > 0)
        {
            if (cam.orthographicSize > minZoom)
            {
                cam.orthographicSize -= zoomIncrement;
                scrollCount++;
            }

        }

        // Scroll Down
        if (scroll.y < 0)
        {
            if (cam.orthographicSize < maxZoom)
            {
                cam.orthographicSize += zoomIncrement;
                scrollCount--;
            }

        }
    }

    // Input
    public void OnMiddleClick(InputAction.CallbackContext context)
    {
        holdingDrag = context.ReadValue<float>() > 0;
        dragOrigin = cam.ScreenToWorldPoint(mousePos);
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
    }

    public void OnScrollWheel(InputAction.CallbackContext context)
    {
        scroll = context.ReadValue<Vector2>();
    }
}
