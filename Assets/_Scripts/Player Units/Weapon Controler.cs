using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WeaponControler : MonoBehaviour
{
    [Header("References")]
    public GameObject gun;
    public GameObject bulletPrefab;
    public GameObject pivot;
    public GameObject shootPoint;
    private Vector2 mouseScreenPos;
    public Image chargeBar;
    [Header("Gun stats")]
    [HideInInspector] public bool isCharging = false;
    private float chargeTime = 0f;
    [SerializeField] private float maxChargeTime = 2.5f; // maximum time the player can charge the shot
    [SerializeField] private float chargeRate = 1f; // how quickly the charge time increases per second
    private float minChargeTime = 0.1f;
    [HideInInspector] public bool shotqueued = false;
    private float finalShotStrength = 0f;
    private float selectionTime = 0f;
    private bool hitMaxCharge = false;
    [HideInInspector] public bool hasShot { get; private set; } = false;

    void FixedUpdate()
    {
        // Update the selection time if this unit is selected and it's the player turn
        if (PointerController.Instance.selectedUnit == this.gameObject && GetComponent<UnitMovement>().isBlue == GameManager.gm.blueTurn)
        {
            selectionTime += Time.fixedDeltaTime;
        }
        else
        {
            selectionTime = 0f;
        }
        
        if (PointerController.Instance.selectedUnit == this.gameObject && shotqueued == false && GetComponent<UnitMovement>().isBlue == GameManager.gm.blueTurn)
        {
            //if is charging then increase the charge time and update the charge bar
            if (isCharging)
            {
                if (chargeTime < maxChargeTime && hitMaxCharge == false)
                {
                    chargeTime += chargeRate * Time.fixedDeltaTime;
                    if (chargeTime >= maxChargeTime) hitMaxCharge = true;

                }
                else if (hitMaxCharge == true)
                {
                    // if hit max charge then start to decrease the charge time and update the charge bar
                    chargeTime -= chargeRate * Time.fixedDeltaTime;
                    if (chargeTime <= 0) hitMaxCharge = false;
                }

                chargeBar.fillAmount = ((chargeTime) / (maxChargeTime));
            }
            if (shotqueued == false && !hasShot)
            {
                RotatePivotToMouse();
            }
            
        }



    }
    private void Update()
    {
        if(PointerController.Instance.selectedUnit != this.gameObject)
        {
            ResetUI();
        }
        if (GameManager.gm.shootTurn == true)
        {
            hasShot = false;
        }
    }

    public void LookAtMouse(InputAction.CallbackContext context)
    {
        // Update the mouse position when the player moves the mouse
        if (PointerController.Instance.selectedUnit == this.gameObject && GetComponent<UnitMovement>().isBlue == GameManager.gm.blueTurn)
        {
            mouseScreenPos = context.ReadValue<Vector2>();
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        var movement = GetComponent<UnitMovement>();
        bool myTurn = movement.isBlue == GameManager.gm.blueTurn;
        if (myTurn && PointerController.Instance.selectedUnit == this.gameObject && !shotqueued && !hasShot && selectionTime > 0.5f&& movement.grounded)
        {
            // Start charging the shot when the player presses the left clik
            if (context.started)
            {
                isCharging = true;
            }
            else if (context.canceled && isCharging && chargeTime >= minChargeTime)
            {
                isCharging = false;
                finalShotStrength = chargeTime;
                shotqueued = true;
                PointerController.Instance.selectedUnit = null;
            }
        }

        if (shotqueued && !hasShot && movement.grounded)
        {
            // Queue the shot with the GameManager and reset the UI
            GameManager.gm.QueueShot(bulletPrefab, shootPoint.transform.position, shootPoint.transform.rotation, finalShotStrength, shootPoint.transform);
            shotqueued = false;
            hasShot = true;
            ResetUI();
        }
    }


    private void RotatePivotToMouse()
    {
        if (PointerController.Instance.selectedUnit == this.gameObject && shotqueued == false && GetComponent<UnitMovement>().isBlue == GameManager.gm.blueTurn)
        {
            // Get the position of the pivot in screen space
            Vector3 pivotScreenPos = Camera.main.WorldToScreenPoint(pivot.transform.position);
            Vector2 direction = mouseScreenPos - (Vector2)pivotScreenPos;
            // Calculate the angle between the pivot and the mouse position
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            pivot.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
        }
    }

    public void ResetUI()
    {
        chargeTime = 0f;
        chargeBar.fillAmount = 0f;
        isCharging = false;
        hitMaxCharge = false;
    }

}
