using System;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjectArchitecture;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "SO/GameLoop/LevelGenerator")]
public class LevelGenerator : ScriptableObject
{
    [SerializeField] private bool isDebug = true;

    [SerializeField] private GameObject planetPrefab;
    [SerializeField] private GameObject oxytankPrefab;
    [SerializeField] private GameObject fuelPrefab;
    [SerializeField] private GameObject[] asteroidPrefabs;


    [SerializeField] private Vector2 debugMainAsteroidFieldSize;
    [SerializeField] private Vector2 debugPlayerStartPoint = Vector2.left * 15f;
    [SerializeField] private int asteroidsDebugNum;
    [SerializeField] private int gasTanksDebugNum;
    [SerializeField] private int oxyTanksDebugNum;


    private List<GameObject> curGameObjects;

    private float distanceToOxyTank;

    public void OnEnable()
    {
        curGameObjects = new List<GameObject>();
    }

    public void GenerateLevel(GameObject player, MinimapCamera mmCamera)
    {
        ClearPreviousShit();

        UseHaltonSequenceMethod(player, mmCamera);
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

        // then do the halton sequence stuff
        GenerateObjectsByHaltonSequence(asteroidsDebugNum, gasTanksDebugNum, oxyTanksDebugNum, debugMainAsteroidFieldSize);

        //then fill in other portions randomly?

    }

    void GenerateObjectsByHaltonSequence(int numAsteroids, int numGasTanks, int numOxyTanks, Vector2 asteroidFieldSize)
    {
        //drop the first 0-20 values
        var goNumber = (int) (Random.value * 20);

        var lowPrimes = new List<int> { 2, 3, 5, 7 }.OrderBy(x => Random.value).ToList();
        var xPrime = lowPrimes[0];
        var yPrime = lowPrimes[1];

        Debug.Log($"{goNumber} {xPrime} {yPrime}");

        //var goNumber = 0;

        for (int i = 0; i < numAsteroids; i++)
        {
            var location = new Vector3(
                GetHaltonSequenceNumber(goNumber, xPrime) * asteroidFieldSize.x,
                GetHaltonSequenceNumber(goNumber, yPrime) * asteroidFieldSize.y - asteroidFieldSize.y/2,
                1f);

            var asteroidPrefabIndex = Mathf.Min(
                (int)(Random.value * asteroidPrefabs.Length),
                asteroidPrefabs.Length - 1);
            

            var asteroidGO = Instantiate(asteroidPrefabs[asteroidPrefabIndex], location, Quaternion.identity);
            curGameObjects.Add(asteroidGO);

            goNumber++;
        }

        for (int i = 0; i < numGasTanks; i++)
        {
            var location = new Vector3(
                GetHaltonSequenceNumber(goNumber, xPrime) * asteroidFieldSize.x,
                GetHaltonSequenceNumber(goNumber, yPrime) * asteroidFieldSize.y - asteroidFieldSize.y / 2,
                1f);

            var gtGO = Instantiate(fuelPrefab, location, Quaternion.identity);
            curGameObjects.Add(gtGO);
            goNumber++;
        }

        for (int i = 0; i < numOxyTanks; i++)
        {
            var location = new Vector3(
                GetHaltonSequenceNumber(goNumber, xPrime) * asteroidFieldSize.x,
                GetHaltonSequenceNumber(goNumber, yPrime) * asteroidFieldSize.y - asteroidFieldSize.y / 2,
                1f);

            var otGO = Instantiate(oxytankPrefab, location, Quaternion.identity);
            curGameObjects.Add(otGO);

            goNumber++;
        }
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
