using System;
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
    [SerializeField] private bool isDebug = true;
    [SerializeField] private bool useHaltonSequence = true;

    [SerializeField] private GameObject planetPrefab;
    [SerializeField] private GameObject oxytankPrefab;
    [SerializeField] private GameObject fuelPrefab;
    [SerializeField] private GameObject[] asteroidPrefabs;


    [SerializeField] private Vector2 debugMainAsteroidFieldSize;
    [SerializeField] private Vector2 debugPlayerStartPoint = Vector2.left * 15f;
    [SerializeField] private int asteroidsDebugNum;
    [SerializeField] private int gasTanksDebugNum;
    [SerializeField] private int oxyTanksDebugNum;


    [SerializeField] private FloatReference assumedVelocity;
    [SerializeField] private FloatReference driftAcceleration;

    [SerializeField] private FloatReference assumedMinAvgVelocityBetweenOxygenTanks;
    [SerializeField] private IntReference numOxytankDistanceToGoal;

    [SerializeField] private FloatReference oxytankSeconds;



    //[SerializeField] public List<PickupItem> pickupItems; 
    private List<GameObject> curGameObjects;

    private float distanceToOxyTank;

    public void OnEnable()
    {
        curGameObjects = new List<GameObject>();
        //pickupItems = new List<PickupItem>();
    }

    public void GenerateLevel(GameObject player, MinimapCamera mmCamera)
    {
        ClearPreviousShit();

        if (useHaltonSequence)
        {
            UseHaltonSequenceMethod(player, mmCamera);
        }
        else
        {
            UseHeuristicMethod(player);
        }
    }

    private void UseHaltonSequenceMethod(GameObject player, MinimapCamera mmCamera)
    {
        // always put a fuel tank at 0,0
        var fuelGO = Instantiate(fuelPrefab);
        curGameObjects.Add(fuelGO);

        // and start player at...some sorta start point ... -20? (10 seconds.. at 2 units/second)
        player.transform.position = debugPlayerStartPoint;

        // place the planet somewhere just outside 
        var planetLocation = new Vector3(
            debugMainAsteroidFieldSize.x + 20f,
            Random.Range(debugMainAsteroidFieldSize.y * -0.45f, debugMainAsteroidFieldSize.y * 0.45f),
            1);

        var planetGO = Instantiate(planetPrefab, planetLocation, Quaternion.identity);
        curGameObjects.Add(planetGO);

        //minimap camera
        mmCamera.SetupMinimapSizing(debugMainAsteroidFieldSize.x);

        //var num
        // then do the halton sequence stuff
        GenerateObjectsByHaltonSequence(asteroidsDebugNum, gasTanksDebugNum, oxyTanksDebugNum, debugMainAsteroidFieldSize);
        //then fill in other portions randomly?

    }

    void GenerateObjectsByHaltonSequence(int numAsteroids, int numGasTanks, int numOxyTanks, Vector2 asteroidFieldSize)
    {
        var goNumber = 0;

        for (int i = 0; i < numAsteroids; i++)
        {
            var location = new Vector3(
                GetHaltonSequenceNumber(goNumber, 2) * asteroidFieldSize.x,
                GetHaltonSequenceNumber(goNumber, 3) * asteroidFieldSize.y - asteroidFieldSize.y/2,
                1f);

            var asteroidGO = Instantiate(asteroidPrefabs[0], location, Quaternion.identity);
            curGameObjects.Add(asteroidGO);

            goNumber++;
        }

        for (int i = 0; i < numGasTanks; i++)
        {
            var location = new Vector3(
                GetHaltonSequenceNumber(goNumber, 2) * asteroidFieldSize.x,
                GetHaltonSequenceNumber(goNumber, 3) * asteroidFieldSize.y - asteroidFieldSize.y / 2,
                1f);

            var gtGO = Instantiate(fuelPrefab, location, Quaternion.identity);
            curGameObjects.Add(gtGO);
            goNumber++;
        }

        for (int i = 0; i < numOxyTanks; i++)
        {
            var location = new Vector3(
                GetHaltonSequenceNumber(goNumber, 2) * asteroidFieldSize.x,
                GetHaltonSequenceNumber(goNumber, 3) * asteroidFieldSize.y - asteroidFieldSize.y / 2,
                1f);

            var otGO = Instantiate(oxytankPrefab, location, Quaternion.identity);
            curGameObjects.Add(otGO);

            goNumber++;
        }
    }

    private void UseHeuristicMethod(GameObject player)
    {
        distanceToOxyTank = oxytankSeconds * assumedMinAvgVelocityBetweenOxygenTanks;


        // start fuel...
        var curPosition = new Vector3(10, 0, 1);

        for (var i = 0; i < numOxytankDistanceToGoal.Value; i++)
        {
            var direction = new Vector2(Random.value, Random.value).normalized;

            var nextOxyTankLocation = curPosition + (Vector3)direction * distanceToOxyTank;

            var oxyTankGO = Instantiate(oxytankPrefab, nextOxyTankLocation, Quaternion.identity);
            //oxyTankGO.GetComponent<Pickupable>().pickupAmt = oxytankSeconds.Value;
            /*
            pickupItems.Add(new PickupItem
            {
                pickupType = PickupType.OxyTank,
                pickupPosition = nextOxyTankLocation
            });*/
            curGameObjects.Add(oxyTankGO);

            curPosition = nextOxyTankLocation;
        }

        var nextDir = new Vector2(Random.value, Random.value).normalized;

        var planetLocation = curPosition + (Vector3)nextDir * distanceToOxyTank;

        var planetGO = Instantiate(planetPrefab, planetLocation, Quaternion.identity);
        curGameObjects.Add(planetGO);
        player.transform.position = new Vector3(-5, 0, 0);
    }

    private void ClearPreviousShit()
    {
        foreach (var go in curGameObjects)
        {
            Destroy(go.gameObject);
        }

        curGameObjects = new List<GameObject>();
    }

    private static float GetHaltonSequenceNumber(int index, int basePrime)
    {
        var f = 1f;
        var r = 0f;

        while (index > 0)
        {
            f /= basePrime;
            r += f * (index % basePrime);
            index /= basePrime;
        }

        return r;
    }
}
