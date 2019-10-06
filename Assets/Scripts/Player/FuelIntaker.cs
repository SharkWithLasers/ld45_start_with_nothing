using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class FuelIntaker : MonoBehaviour
{
    [SerializeField] private FloatReference currentFuel;
    [SerializeField] private FloatReference maxFuel;

    [SerializeField] private float initialFuel = 0f;

    [SerializeField] private GameObject fuelDepletedTextPrefab;

    // Update is called once per frame
    public void OnFuelPickedUp(float amt)
    {
        currentFuel.Value = Mathf.Min(maxFuel, currentFuel.Value + amt);
    }

    public void OnLevelStart()
    {
        currentFuel.Value = initialFuel;
    }

    public void OnFuelDepleted()
    {
        var fuelDepletedGO = Instantiate(fuelDepletedTextPrefab, transform);
        fuelDepletedGO.transform.localPosition = new Vector3(0, 0.6f, 0);
    }
}
