using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using Random = UnityEngine.Random;

public enum PickupType
{
    OxyTank,
    Fuel
}

[Serializable]
public class PickupItem
{
    public PickupType pickupType;
    public Vector2 pickupPosition;
}

[CreateAssetMenu(menuName = "SO/GameLoop/LevelGenerator")]
public class LevelGenerator : ScriptableObject
{
    [SerializeField] private FloatReference assumedVelocity;
    [SerializeField] private FloatReference driftAcceleration;

    [SerializeField] private FloatReference assumedMinAvgVelocityBetweenOxygenTanks;
    [SerializeField] private IntReference numOxytankDistanceToGoal;

    [SerializeField] private GameObject planetPrefab;
    [SerializeField] private GameObject oxytankPrefab;
    [SerializeField] private GameObject fuelPrefab;

    [SerializeField] private FloatReference oxytankSeconds;

    [SerializeField] public List<PickupItem> pickupItems; 

    //private Dictionary<>

    private float distanceToOxyTank;

    public void OnEnable()
    {
        pickupItems = new List<PickupItem>();
        //Debug.Log(distanceToOxyTank);
    }

    public void GenerateLevel()
    {
        distanceToOxyTank = oxytankSeconds * assumedMinAvgVelocityBetweenOxygenTanks;


        // start fuel...
        var curPosition = new Vector2(10, 0);

        for (var i = 0; i < numOxytankDistanceToGoal.Value; i++)
        {
            var direction = new Vector2(Random.value, Random.value).normalized;

            var nextOxyTankLocation = curPosition + direction * distanceToOxyTank;

            Instantiate(oxytankPrefab, nextOxyTankLocation, Quaternion.identity);
            pickupItems.Add(new PickupItem
            {
                pickupType = PickupType.OxyTank,
                pickupPosition = nextOxyTankLocation
            });
            curPosition = nextOxyTankLocation;
        }

    }
}
