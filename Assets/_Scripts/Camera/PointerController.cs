using UnityEngine;
using UnityEngine.InputSystem;
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

    [SerializeField]
    private PointerMarker pointerMarker;
    [SerializeField]
    private Sprite canPlaceMarker;
    [SerializeField]
    private Sprite cannotPlaceMarker;
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
    private bool canPlaceObject = false;
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

    private void Update()
    {
        CameraMovement();        

        if (targetObject != null)
        {
            CheckPlacement();
            ObjectMovement();
        }
            

        if (!gm.inPlay && isFollowing)
        {
            StopFollowPointer();
        }
    }

    // Camera movement and zooming
    private void CameraMovement()
    {
        // Only allow camera movement and zooming if the game is in play mode.
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
        // Reset camera position and zoom if the game is not in play mode to default.
        else
        {
            camParent.transform.position = defaultPos.position;
            cam.orthographicSize = maxZoom;
            scrollCount = 0;
        }
    }

    // Object movement to follow pointer with grid snapping.
    private void ObjectMovement()
    {
        if (isFollowing && targetObject != null)
        {
            ObjectController obj = targetObject.GetComponent<ObjectController>();

            int width = obj.WorldSize.x;
            int height = obj.WorldSize.y;

            Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);

            Vector3 snapped = new Vector3(SnapAxis(worldPos.x, width), SnapAxis(worldPos.y, height), 0f);

            targetObject.transform.position = snapped;
            pointerMarker.transform.position = snapped;
        }
    }

    // Snap the object to the grid based on its size.
    private float SnapAxis(float value, int size)
    {
        return size % 2 == 0 ? Mathf.Round(value) : Mathf.Floor(value) + 0.5f;
    }

    private void ObjectRotate(float degrees)
    {
        float z = Mathf.Round((targetObject.transform.eulerAngles.z + degrees) / 90f) * 90f;
        targetObject.transform.rotation = Quaternion.Euler(0f, 0f, z);
    }

    // Checking if object can be placed
    private void CheckPlacement()
    {
        if (!isFollowing || targetObject == null) return;

        ObjectController obj = targetObject.GetComponent<ObjectController>();

        int width = obj.WorldSize.x;
        int height = obj.WorldSize.y;

        Vector3 origin = obj.center - new Vector3(width / 2f, height / 2f, 0f);

        string collTag = gm.turnCount % 2 == 0 ? "TeamASection" : "TeamBSection";
        string invCollTag = gm.turnCount % 2 == 0 ? "TeamBSection" : "TeamASection";

        bool inTeamSection = true;
        bool overlapping = false;
        bool overGround = true;

        // Check each tile the object would occupy to see if it can be placed.
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 checkPos = origin + new Vector3(x + 0.5f, y + 0.5f, 0f);

                if (!obj.objectData.isWall)
                {
                    if (!CanPlaceObject(checkPos, collTag, null))
                        inTeamSection = false;
                    if (CanPlaceObject(checkPos, "Object", "Ground"))
                        overlapping = true;
                }

                else
                {
                    if (CanPlaceObject(checkPos, invCollTag, null))
                        inTeamSection = false;
                    if (CanPlaceObject(checkPos, "Object", "Ground"))
                        overlapping = true;
                }
            }
        }

        // Check if the object is over ground tiles.
        for (int x = 0; x < width; x++)
        {
            if (!obj.objectData.isWall)
            {
                Vector3 below = origin + new Vector3(x + 0.5f, -0.5f, 0f);

                if (!CanPlaceObject(below, "Ground", "Object"))
                    overGround = false;
            }
            else
                overGround = true;                
        }

        bool valid = inTeamSection && !overlapping && overGround;

        canPlaceObject = valid;
        pointerMarker.SetMarker(valid ? canPlaceMarker : cannotPlaceMarker, valid ? Color.green : Color.red);
    }

    // Check if the object can be placed at the given position by checking for collisions with other objects or ground tiles.
    private bool CanPlaceObject(Vector3 position, string tagA, string tagB)
    {
        Collider[] cols = Physics.OverlapBox(position, new Vector3(0.1f, 0.1f, 50f));

        foreach (Collider col in cols)
        {
            if (col.CompareTag(tagA) || (tagB != null && col.CompareTag(tagB)))
                return true;
        }
        return false;
    }

    // Stop object from following the pointer and place it in the world.
    private void PlaceObject()
    {
        if (canPlaceObject)
        {
            targetObject.GetComponent<ObjectController>().PlaceObject();

            targetObject = null;
            isFollowing = false;

            canPlaceObject = false;

            pointerMarker.SetMarker(null, Color.white);
        }
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

        targetObject = null;
        isFollowing = false;

        canPlaceObject = false;

        pointerMarker.SetMarker(null, Color.white);
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
    

    public void OnKeyPressE(InputAction.CallbackContext context)
    {
        if (context.performed && targetObject != null)
        {
            ObjectRotate(90);
        }
    }

    public void OnKeyPressQ(InputAction.CallbackContext context)
    {
        if (context.performed && targetObject != null)
        {
            ObjectRotate(-90);
        }
    }
}
