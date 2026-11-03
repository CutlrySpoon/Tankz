using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WeaponControler : MonoBehaviour
{
    public GameObject gun;
    public GameObject bulletPrefab;
    public GameObject pivot;
    public GameObject shootPoint;
    private Vector2 mouseScreenPos;
    public Image chargeBar;
    private bool isCharging = false;
    [SerializeField] private float chargeTime = 0f;
    [SerializeField] private float maxChargeTime = 2.5f;
    [SerializeField] private float chargeRate = 1f;
    private bool shotqueued = false;

    void FixedUpdate()
    {
        if(1==1)//add condition for selected unit
        {
            
            if (isCharging)
            {
                
                chargeTime = Mathf.PingPong(Time.time * chargeRate, maxChargeTime) + 1;

                chargeBar.fillAmount = ((chargeTime - 1) / (maxChargeTime));
            }
            else if (shotqueued == false)
            {
                RotatePivotToMouse();
            }
            if (shotqueued)// && enter conditon for shoot turn
            {
                Instantiate(bulletPrefab, shootPoint.transform.position, shootPoint.transform.rotation);
                shotqueued = false;
                
            }
        }

    }

    public void LookAtMouse(InputAction.CallbackContext context)
    {
        if (1 == 1)//add condition for selected unit
        {
            mouseScreenPos = context.ReadValue<Vector2>();
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (1 == 1)//add condition for selected unit
        {
            if (context.started)
            {
                isCharging = true;

            }
            if (context.canceled)
            {
               
                isCharging = false;
                bulletPrefab.GetComponent<BulletBehaviour>().shotStregth = chargeTime;
                chargeTime = 0;
                chargeBar.fillAmount = 0f;
                shotqueued = true;
            }
        }

    }


    private void RotatePivotToMouse()
    {
        if (1 == 1)//add condition for selected unit
        {
            // Get the position of the pivot in screen space
            Vector3 pivotScreenPos = Camera.main.WorldToScreenPoint(pivot.transform.position);
            Vector2 direction = mouseScreenPos - (Vector2)pivotScreenPos;
            // Calculate the angle between the pivot and the mouse position
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            pivot.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
        }
    }



}
