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
    private float mostRecentHorzX;
    private Option<Vector2> prevDirection = Option<Vector2>.None;

    // Start is called before the first frame update
    void Start()
    {
        movementController = GetComponent<MovementController>();
        mostRecentHorzX = 1f;
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

        var inputVec = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"));
        //var horzInput = Input.GetAxisRaw("Horizontal");
        //var vertInput = Input.GetAxisRaw("Vertical");

        var acceleratePressed = Input.GetButtonDown("Accelerate");
        var accelerateHeld = Input.GetButton("Accelerate");

        var inputHeld = inputVec.sqrMagnitude > 0f;

        if (accelerateHeld && fuelInUnits > 0f)
        {
            var inputPressed = Input.GetButtonDown("Up") || Input.GetButtonDown("Right")
                || Input.GetButtonDown("Left") || Input.GetButtonDown("Down");

            /*var inputPressed = Input.GetButtonDown("Up") || Input.GetButtonDown("Right")
                || Input.GetButtonDown("Left") || Input.GetButtonDown("Down") ||
                Input.GetButtonUp("Up") || Input.GetButtonUp("Right")
                || Input.GetButtonUp("Left") || Input.GetButtonUp("Down");*/

            var unitDirection = inputHeld
                ? inputVec.normalized
                : new Vector2(mostRecentHorzX, 0f);

            if (acceleratePressed)
            {
                var dotProd = Vector2.Dot(unitDirection, currentPlayerDriftVelocity.Value);


                var driftSpeedToUse = Mathf.Approximately(dotProd, 1f)
                    ? currentPlayerDriftVelocity.Value.magnitude
                    : dotProd > 0
                        ? currentPlayerDriftVelocity.Value.magnitude * acuteDirectionRatio
                        : minDriftSpeed;

                /*var dotProd = !prevDirection.HasValue
                    ? minDriftSpeed
                    : Vector2.Dot(inputVec, prevDirection.Value);

                var driftSpeedToUse = dotProd == 1
                    ? currentPlayerDriftVelocity.Value.magnitude
                    : dotProd > 0
                        ? currentPlayerDriftVelocity.Value.magnitude * acuteDirectionRatio
                        : minDriftSpeed;*/

                currentPlayerDriftVelocity.Value = unitDirection * driftSpeedToUse;
            }

            var accelerationToUse = Mathf.Min(fuelInUnits, driftAcceleration * Time.deltaTime);

            currentPlayerDriftVelocity.Value = Vector2.ClampMagnitude(
                currentPlayerDriftVelocity + unitDirection * accelerationToUse,
                maxDriftSpeed);

            fuelInUnits.Value = Mathf.Max(0f, fuelInUnits.Value - accelerationToUse);

        }

        mostRecentHorzX = Mathf.Approximately(inputVec.x, 0f)
            ? mostRecentHorzX
            : inputVec.x;

        prevDirection = inputHeld
            ? inputVec
            : prevDirection;

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
