using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class PlayerMovementInput : MonoBehaviour
{
    [SerializeField] private BoolReference inDebugMode;

    [SerializeField] private FloatReference moveSpeed;

    [SerializeField] private FloatReference fuelInSeconds;

    [SerializeField] private BoolReference useDriftMode;

    [SerializeField] private FloatReference driftAcceleration;
    [SerializeField] private Vector2 initialDriftVelocity;
    [SerializeField] private Vector2Reference currentPlayerDriftVelocity;

    [SerializeField] private FloatReference minDriftSpeed;
    [SerializeField] private FloatReference maxDriftSpeed;


    [SerializeField] private FloatReference acuteDirectionRatio;

    [SerializeField] private GameEvent fuelDepletedEvent;


    [SerializeField] private Thruster thruster;

    private float mostRecentHorzX;
    private Option<Vector2> prevDirection = Option<Vector2>.None;

    private bool levelRunning = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (inDebugMode == true)
        {
            DebugFlow();
        }

        if (levelRunning)
        {
            DriftModeFlow();
        }
    }

    private void DriftModeFlow()
    {

        var inputVec = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"));

        var acceleratePressed = Input.GetButtonDown("Accelerate");
        var accelerateHeld = Input.GetButton("Accelerate");

        var inputHeld = inputVec.sqrMagnitude > 0f;

        if (accelerateHeld && fuelInSeconds > 0f)
        {
            thruster.TryStartThrusting();

            var inputPressed = Input.GetButtonDown("Up") || Input.GetButtonDown("Right")
                || Input.GetButtonDown("Left") || Input.GetButtonDown("Down");

            // once reason for weirdness is that when this is pressed before accelerate removed, shit gets weird
            /*var inputPressed = Input.GetButtonDown("Up") || Input.GetButtonDown("Right")
                || Input.GetButtonDown("Left") || Input.GetButtonDown("Down") ||
                Input.GetButtonUp("Up") || Input.GetButtonUp("Right")
                || Input.GetButtonUp("Left") || Input.GetButtonUp("Down");*/

            var unitDirection = inputHeld
                ? inputVec.normalized
                : new Vector2(mostRecentHorzX, 0f);

            if (acceleratePressed || inputPressed)
            {
                var dotProd = Vector2.Dot(unitDirection, currentPlayerDriftVelocity.Value.normalized);

                var driftSpeedToUse = Mathf.Approximately(dotProd, 1f)
                    ? currentPlayerDriftVelocity.Value.magnitude
                    : dotProd > 0f
                        ? currentPlayerDriftVelocity.Value.magnitude * acuteDirectionRatio
                        : Mathf.Approximately(dotProd, 0f) ? currentPlayerDriftVelocity.Value.magnitude * 0.5f : minDriftSpeed;


                currentPlayerDriftVelocity.Value = unitDirection * driftSpeedToUse;
            }

            // average turn is 0.25 seconds?
            // average acceleration turn might be change of 5 / 5 ... 1 second?

            var fuelToUse = Mathf.Min(fuelInSeconds.Value, Time.deltaTime);
            var changeInVelocity = unitDirection * driftAcceleration.Value * fuelToUse;

            currentPlayerDriftVelocity.Value = Vector2.ClampMagnitude(
                currentPlayerDriftVelocity + changeInVelocity,
                maxDriftSpeed);

            fuelInSeconds.Value = Mathf.Max(0f, fuelInSeconds.Value - fuelToUse);
            if (Mathf.Approximately(fuelInSeconds.Value, 0f))
            {
                Debug.Log("fuel is gone");
                fuelDepletedEvent.Raise();
            }
        }
        else
        {
            thruster.TryStopThrusting();
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
            fuelInSeconds.Value += 5f;
        }
    }

    public void OnLevelEnded()
    {
        levelRunning = false;
        currentPlayerDriftVelocity.Value = Vector2.zero;
    }

    public void OnLevelStarted()
    {
        mostRecentHorzX = 1f;
        currentPlayerDriftVelocity.Value = initialDriftVelocity;
        levelRunning = true;
    }
}
