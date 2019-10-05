using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class PlayerMovementInput : MonoBehaviour
{
    [SerializeField] private BoolReference inDebugMode;

    [SerializeField] private FloatReference moveSpeed;
    [SerializeField] private FloatReference fuelInUnits;

    [SerializeField] private BoolReference useDriftMode;

    private MovementController movementController;


    [SerializeField] private FloatReference driftAcceleration;
    [SerializeField] private Vector2 initialDriftVelocity;
    [SerializeField] private Vector2Reference currentPlayerDriftVelocity;

    [SerializeField] private FloatReference minDriftSpeed;
    [SerializeField] private FloatReference maxDriftSpeed;

    [SerializeField] private FloatReference acuteDirectionRatio;


    // Start is called before the first frame update
    void Start()
    {
        movementController = GetComponent<MovementController>();
        currentPlayerDriftVelocity.Value = initialDriftVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        if (inDebugMode == true)
        {
            DebugFlow();
        }


        if (useDriftMode)
        {
            DriftModeFlow();
        }

        else
        {
            ShmupFlow();
        }
    }

    private void ShmupFlow()
    {
        var horzInput = Input.GetAxisRaw("Horizontal");
        var vertInput = Input.GetAxisRaw("Vertical");
        if (horzInput == 0f && vertInput == 0f)
        {
            return;
        }

        var unclampedDisplacement = new Vector2(
            horzInput,
            vertInput).normalized * moveSpeed * Time.deltaTime;

        var clampedDisplacement = Vector2.ClampMagnitude(unclampedDisplacement, fuelInUnits);

        movementController.TryDisplace(clampedDisplacement, transform);

        fuelInUnits.Value = Mathf.Max(0f, fuelInUnits.Value - clampedDisplacement.magnitude);
    }

    private void DriftModeFlow()
    {
        var horzInput = Input.GetAxisRaw("Horizontal");
        var vertInput = Input.GetAxisRaw("Vertical");

        var inputHeld = horzInput != 0f || vertInput != 0f;

        if (inputHeld && fuelInUnits > 0f)
        {
            var inputPressed = Input.GetButtonDown("Up") || Input.GetButtonDown("Right")
                || Input.GetButtonDown("Left") || Input.GetButtonDown("Down");
            
            /*var inputPressed = Input.GetButtonDown("Up") || Input.GetButtonDown("Right")
                || Input.GetButtonDown("Left") || Input.GetButtonDown("Down") ||
                Input.GetButtonUp("Up") || Input.GetButtonUp("Right")
                || Input.GetButtonUp("Left") || Input.GetButtonUp("Down");*/

            var unitDirection = new Vector2(horzInput, vertInput).normalized;

            if (inputPressed)
            {
                var driftSpeedToUse = Vector2.Dot(unitDirection, currentPlayerDriftVelocity.Value) <= 0f
                    ? minDriftSpeed
                    : currentPlayerDriftVelocity.Value.magnitude * acuteDirectionRatio;

                currentPlayerDriftVelocity.Value = unitDirection * driftSpeedToUse;
            }

            var accelerationToUse = Mathf.Min(fuelInUnits, driftAcceleration * Time.deltaTime);

            currentPlayerDriftVelocity.Value = Vector2.ClampMagnitude(
                currentPlayerDriftVelocity + unitDirection * accelerationToUse,
                maxDriftSpeed);

            fuelInUnits.Value = Mathf.Max(0f, fuelInUnits.Value - accelerationToUse);

        }

        var displacement = currentPlayerDriftVelocity.Value * Time.deltaTime;
        transform.Translate(displacement);
    }

    void DebugFlow()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            fuelInUnits.Value += 1000f;
        }
    }
}
