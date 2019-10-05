using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

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

    private float distanceToOxyTank;

    public void Awake()
    {
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

            curPosition = nextOxyTankLocation;
        }

    }
}
