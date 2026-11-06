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
    public bool isCharging = false;
    [SerializeField] private float chargeTime = 0f;
    [SerializeField] private float maxChargeTime = 2.5f;
    [SerializeField] private float chargeRate = 1f;
    [SerializeField] private float minChargeTime = 0.1f;
    public bool shotqueued = false;
    private float finalShotStrength = 0f;
    private float selectionTime = 0f;

    void FixedUpdate()
    {
        if(PointerController.Instance.selectedUnit == this.gameObject && GetComponent<UnitMovement>().isBlue == GameManager.gm.blueTurn)//add condition for selected unit
        {
            selectionTime += Time.fixedDeltaTime;
        }
        else
        {
            selectionTime = 0f;
        }
        selectionTime += Time.fixedDeltaTime;
        if (PointerController.Instance.selectedUnit == this.gameObject && shotqueued == false && GetComponent<UnitMovement>().isBlue == GameManager.gm.blueTurn)//add condition for selected unit
        {
            
            if (isCharging)
            {
                if(chargeTime < maxChargeTime)
                {
                    chargeTime += chargeRate * Time.fixedDeltaTime;
                }
                else
                {
                    chargeTime -= chargeRate * Time.fixedDeltaTime;
                }


                chargeBar.fillAmount = ((chargeTime) / (maxChargeTime));
            }
            if (shotqueued == false)
            {
                RotatePivotToMouse();
            }
            
        }



    }
    private void Update()
    {
        if (shotqueued == true)
        {

            if (GameManager.gm.shootTurn)
            {
                GameObject newBullet = Instantiate(bulletPrefab, shootPoint.transform.position, shootPoint.transform.rotation);

                // Pass the stored charge strength to the bullet
                BulletBehaviour bulletScript = newBullet.GetComponent<BulletBehaviour>();
                if (bulletScript != null)
                {
                    bulletScript.shotStregth = finalShotStrength;
                }


                // Reset after the shot
                shotqueued = false;
                chargeTime = 0;
                chargeBar.fillAmount = 0f;
            }
        }
    }
    public void LookAtMouse(InputAction.CallbackContext context)
    {
        if (PointerController.Instance.selectedUnit == this.gameObject && GetComponent<UnitMovement>().isBlue == GameManager.gm.blueTurn)//add condition for selected unit
        {
            mouseScreenPos = context.ReadValue<Vector2>();
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {

            if (PointerController.Instance.selectedUnit == this.gameObject && shotqueued == false && selectionTime > 0.5f)
            {
                if (GetComponent<UnitMovement>().isBlue == GameManager.gm.blueTurn)
                {
                    if (context.started)
                    {
                        isCharging = true;
                    }
                }

                if (context.canceled && minChargeTime <= chargeTime)
                {
                    isCharging = false;

                    finalShotStrength = chargeTime;

                    shotqueued = true;
                    PointerController.Instance.selectedUnit = null;
                }
            }

    }


    private void RotatePivotToMouse()
    {
        if (PointerController.Instance.selectedUnit == this.gameObject && shotqueued == false && GetComponent<UnitMovement>().isBlue == GameManager.gm.blueTurn)//add condition for selected unit
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
