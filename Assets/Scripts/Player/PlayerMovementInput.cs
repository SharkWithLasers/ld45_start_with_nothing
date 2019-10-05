using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class PlayerMovementInput : MonoBehaviour
{
    [SerializeField] private BoolReference inDebugMode;

    [SerializeField] private FloatReference moveSpeed;
    [SerializeField] private FloatReference fuelInUnits;

    private MovementController movementController;


    // Start is called before the first frame update
    void Start()
    {
        movementController = GetComponent<MovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inDebugMode == true)
        {
            DebugFlow();
        }

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

    void DebugFlow()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            fuelInUnits.Value += 10f;
        }
    }
}
