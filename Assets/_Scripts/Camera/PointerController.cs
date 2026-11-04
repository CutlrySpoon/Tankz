using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static GameManager;
using static ResourceManager;

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


    private void Start()
    {
        cam.orthographicSize = maxZoom;
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
        ObjectMovement();

        CheckPlacement();

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

    // Object movement to follow pointer.
    private void ObjectMovement()
    {
        if (isFollowing && targetObject != null)
        {
            Vector3 pointerPosition = Camera.main.ScreenToWorldPoint(mousePos);
            pointerPosition = new Vector3(Mathf.Ceil(pointerPosition.x) - 0.5f, Mathf.Ceil(pointerPosition.y) - 0.5f, 0f);
            targetObject.transform.position = pointerPosition;
            pointerMarker.transform.position = pointerPosition;
        }
    }

    // Checking if object can be placed
    private void CheckPlacement()
    {
        if (isFollowing && targetObject != null)
        {
            bool inTeamSection = false;
            bool overGround = false;
            bool isGrounded = false;
            bool isTeamATurn = gm.turnCount % 2 == 0;

            Ray ray = cam.ScreenPointToRay(mousePos);

            //Check if the pointer is over a team section and not over ground.
            foreach (RaycastHit hit in Physics.RaycastAll(ray, 100))
            {
                Collider col = hit.collider;

                if (isTeamATurn && col.CompareTag("TeamASection"))
                    inTeamSection = true;

                else if (!isTeamATurn && col.CompareTag("TeamBSection"))
                    inTeamSection = true;

                else if (col.CompareTag("Ground") || col.CompareTag("Object"))
                    overGround = true;
            }            

            // Check one tile below to see if the object is being placed on ground.
            Ray offsetRay = new Ray(ray.origin + Vector3.down * 1f, ray.direction);

            foreach (RaycastHit hit in Physics.RaycastAll(offsetRay, 100))
            {
                if(hit.collider.CompareTag("Ground") || hit.collider.CompareTag("Object"))
                {
                    isGrounded = true;
                    break;
                }
            }


            bool valid = inTeamSection && !overGround && isGrounded;

            // Set UI marker to show if the obejct can be placed or not.
            canPlaceObject = valid;
            pointerMarker.SetMarker(valid ? canPlaceMarker : cannotPlaceMarker, valid ? Color.green : Color.red);
        }
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
        if (context.performed && isFollowing)
        {
            PlaceObject();
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
