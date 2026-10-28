using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponControler : MonoBehaviour
{
    public GameObject gun;
    public GameObject bulletPrefab;
    public GameObject pivot;
    public GameObject shootPoint;
    private Vector2 mouseScreenPos;

    void Update()
    {
        RotatePivotToMouse();
    }

    public void LookAtMouse(InputAction.CallbackContext context)
    {
        mouseScreenPos = context.ReadValue<Vector2>();
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        Instantiate(bulletPrefab, shootPoint.transform.position, shootPoint.transform.rotation);
    }



    private void RotatePivotToMouse()
    {

        // Get the position of the pivot in screen space
        Vector3 pivotScreenPos = Camera.main.WorldToScreenPoint(pivot.transform.position);
        Vector2 direction = mouseScreenPos - (Vector2)pivotScreenPos;
        // Calculate the angle between the pivot and the mouse position
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        pivot.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
    }



}
