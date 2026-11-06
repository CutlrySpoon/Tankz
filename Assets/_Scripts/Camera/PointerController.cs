using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static GameManager;

// This is a pointer controller which is used for camera movement and object placement.

// By OS.

public class PointerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private GameObject camParent;
    [SerializeField]
    private Transform defaultPos;
    [Space(5)]

    [SerializeField]
    private ResourceButton resourceButton;
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


    private GameObject targetObject;
    private bool isFollowing = false;
    public GameObject selectedUnit = null;
    public static PointerController Instance;
    private LayerMask unitLayerMask;

    private void Start()
    {
        unitLayerMask = LayerMask.GetMask("Units");
        cam.orthographicSize = maxZoom;
        Instance = this;
    }

    // Clamp the camera max position based on the map size and zoom level
    private void LateUpdate()
    {
        float minX = TileMapManager.tm.mapWidth / 2 - scrollCount * (zoomIncrement * 1.6f);
        float maxX = TileMapManager.tm.mapWidth / 2 + scrollCount * (zoomIncrement * 1.6f);

        float minY = TileMapManager.tm.mapHeight / 2 - scrollCount * (zoomIncrement * 0.8f);
        float maxY = TileMapManager.tm.mapHeight / 2 + scrollCount * (zoomIncrement * 0.8f);

        Vector3 clampPos = camParent.transform.position;

        clampPos.x = Mathf.Clamp(clampPos.x, minX, maxX);
        clampPos.y = Mathf.Clamp(clampPos.y, minY, maxY);

        camParent.transform.position = clampPos;        
    }

    // Dragging and scrolling
    private void Update()
    {
        if (gm.inPlay)
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
        else
        {
            camParent.transform.position = defaultPos.position;
            cam.orthographicSize = maxZoom;
            scrollCount = 0;
        }

        if (isFollowing && targetObject != null)
        {
            Vector3 pointerPosition = Camera.main.ScreenToWorldPoint(mousePos);
            pointerPosition = new Vector3(Mathf.Ceil(pointerPosition.x) - 0.5f, Mathf.Ceil(pointerPosition.y) - 0.5f, 0f);
            targetObject.transform.position = pointerPosition;
        }
    }

    // Stop object from following the pointer and place it in the world.
    private void PlaceObject()
    {
        isFollowing = false;
        targetObject = null;
    }

    // Start following the pointer with the given object.
    public void FollowPointer(GameObject obj)
    {
        resourceButton.CloseMenu();
        targetObject = obj;
        this.isFollowing = true;
    }

    // Stop following the pointer and destroy the object.
    public void StopFollowPointer()
    {
        Destroy(targetObject);
        this.isFollowing = false;
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

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        Ray ray = cam.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (context.performed && isFollowing)
        {
            PlaceObject();
        }
        if(context.performed && !isFollowing && Physics.Raycast(ray, out hit, Mathf.Infinity, unitLayerMask))
        {
            selectedUnit = hit.collider.gameObject;
            Debug.Log("Selected Unit: " + selectedUnit.name);
        }
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StopFollowPointer();
        }
    }
    
}
