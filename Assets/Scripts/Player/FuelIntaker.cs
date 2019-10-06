using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class FuelIntaker : MonoBehaviour
{
    [SerializeField] private FloatReference currentFuel;
    [SerializeField] private FloatReference maxFuel;

    [SerializeField] private float initialFuel = 0f;

    // Update is called once per frame
    public void OnFuelPickedUp(float amt)
    {
        currentFuel.Value = Mathf.Min(maxFuel, currentFuel.Value + amt);
    }

    public void OnLevelStart()
    {
        currentFuel.Value = initialFuel;
    }
}
