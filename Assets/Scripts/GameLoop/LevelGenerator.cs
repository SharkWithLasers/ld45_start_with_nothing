using System;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjectArchitecture;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct DifficultySettings
{
    public float asteroidFieldSize;

    public float playerStartPointX;
    public float initialOxygen;
    public float initialFuel;


    public int numAsteroids;
    public int numGasTanks;
    public int numOxyTanks;
}

[CreateAssetMenu(menuName = "SO/GameLoop/LevelGenerator")]
public class LevelGenerator : ScriptableObject
{
    [SerializeField] private bool isDebug = true;

    [SerializeField] private GameObject planetPrefab;
    [SerializeField] private GameObject oxytankPrefab;
    [SerializeField] private GameObject fuelPrefab;
    [SerializeField] private GameObject firstFuelPrefab;

    [SerializeField] private GameObject[] asteroidPrefabs;

    [SerializeField] private FloatReference playerOxygen;
    [SerializeField] private FloatReference playerFuel;


    [SerializeField] private Vector2 debugMainAsteroidFieldSize;
    [SerializeField] private Vector2 debugPlayerStartPoint = Vector2.left * 15f;
    [SerializeField] private int asteroidsDebugNum;
    [SerializeField] private int gasTanksDebugNum;
    [SerializeField] private int oxyTanksDebugNum;

    [SerializeField] private DifficultySettings initialDifficultySettings;

    private DifficultySettings curDifficultySettings;

    private List<GameObject> curGameObjects;

    private float distanceToOxyTank;


    private void OnEnable()
    {
        Debug.Log("on enable called for so");
        curGameObjects = new List<GameObject>();
        curDifficultySettings = initialDifficultySettings;
    }

    public void GenerateLevel(GameObject player, MinimapCamera mmCamera, Option<bool> shouldBeEasier,
        bool isTutorialLevel)
    {
        ClearPreviousShit();

        // generate difficulty settings
        if (shouldBeEasier.HasValue)
        {
            AdjustDifficultySettings(shouldBeEasier.Value);
        }
        var difficultySettingsToUse = isDebug
            ? GetDifficultySettingsFromDebug()
            : curDifficultySettings;

        UseHaltonSequenceMethod(player, mmCamera, difficultySettingsToUse, isTutorialLevel);
    }

    private void AdjustDifficultySettings(bool shouldBeEaiser)
    {
        if (shouldBeEaiser)
        {
            MakeEasier();
        }
        else
        {
            MakeHarder();
        }
    }

    private void MakeEasier()
    {
        var shouldIncreaseTanks = Random.value > 0.5f;
        if (shouldIncreaseTanks)
        {
            curDifficultySettings.numGasTanks = Mathf.FloorToInt(curDifficultySettings.numGasTanks * 1.25f);
            curDifficultySettings.numOxyTanks = Mathf.FloorToInt(curDifficultySettings.numOxyTanks * 1.25f);
        }
        else
        {
            //should decrease asteroids
            curDifficultySettings.numAsteroids = Mathf.CeilToInt(curDifficultySettings.numAsteroids / 1.25f);
        }
    }

    private void MakeHarder()
    {
        var shouldDecreaseTanks = Random.value > 0.5f;
        if (shouldDecreaseTanks)
        {
            curDifficultySettings.numGasTanks = Mathf.CeilToInt(curDifficultySettings.numGasTanks / 1.25f);
            curDifficultySettings.numOxyTanks = Mathf.CeilToInt(curDifficultySettings.numOxyTanks / 1.25f);
        }
        else
        {
            //should increase asteroids
            curDifficultySettings.numAsteroids = Mathf.FloorToInt(curDifficultySettings.numAsteroids * 1.25f);
        }
    }
    private DifficultySettings GetDifficultySettingsFromDebug()
    {
        return new DifficultySettings
        {
            asteroidFieldSize = debugMainAsteroidFieldSize.x,
            playerStartPointX = debugPlayerStartPoint.x,
            numAsteroids = asteroidsDebugNum,
            numGasTanks = gasTanksDebugNum,
            numOxyTanks = oxyTanksDebugNum
        };
    }

    private void UseHaltonSequenceMethod(
        GameObject player,
        MinimapCamera mmCamera,
        DifficultySettings ds,
        bool isTutorialLevel)
    {
        Debug.Log($"numast:{ds.numAsteroids}..numGt:{ds.numGasTanks}..numOt:{ds.numOxyTanks}");

        // always put a fuel tank at 0,-1
        var fuelGO = Instantiate(firstFuelPrefab, new Vector3(-1, 0, 1f), Quaternion.identity);
        //fuelGO.GetComponent<Fuel>
        curGameObjects.Add(fuelGO);

        // and start player at...some sorta start point ... -20? (10 seconds.. at 2 units/second)
        if (isTutorialLevel)
        {
            player.transform.position = new Vector3(ds.playerStartPointX, 0f, 0f);
            playerOxygen.Value = ds.initialOxygen;
            playerFuel.Value = ds.initialFuel;
            //player.transform.position = new Vector3(ds.playerStartPointX - 20f, 0f, 0f);
            //playerOxygen.Value = ds.initialOxygen + 15f;
            //playerFuel.Value = ds.initialFuel;
        } else
        {
            player.transform.position = new Vector3(ds.playerStartPointX, 0f, 0f);
            playerOxygen.Value = ds.initialOxygen;
            playerFuel.Value = ds.initialFuel;
        }
        

        // place the planet somewhere just outside 
        var planetLocation = new Vector3(
            ds.asteroidFieldSize + 20f,
            Random.Range(ds.asteroidFieldSize * -0.45f, ds.asteroidFieldSize * 0.45f),
            1);

        var planetGO = Instantiate(planetPrefab, planetLocation, Quaternion.identity);
        curGameObjects.Add(planetGO);

        //minimap camera
        mmCamera.SetupMinimapSizing(ds.asteroidFieldSize);

        // then do the halton sequence stuff
        GenerateObjectsByHaltonSequence(ds.numAsteroids, ds.numGasTanks, ds.numOxyTanks, ds.asteroidFieldSize);

        //then fill in other portions randomly? ehh fuck it

    }

    void GenerateObjectsByHaltonSequence(int numAsteroids, int numGasTanks, int numOxyTanks, float asteroidFieldSize)
    {
        //drop the first 0-20 values
        var goNumber = (int) (Random.value * 20);

        var lowPrimes = new List<int> { 2, 3, 5 }.OrderBy(x => Random.value).ToList();
        var xPrime = lowPrimes[0];
        var yPrime = lowPrimes[1];

        //Debug.Log($"{goNumber} {xPrime} {yPrime}");

        //var goNumber = 0;

        for (int i = 0; i < numAsteroids; i++)
        {
            var location = new Vector3(
                GetHaltonSequenceNumber(goNumber, xPrime) * asteroidFieldSize,
                GetHaltonSequenceNumber(goNumber, yPrime) * asteroidFieldSize - asteroidFieldSize / 2,
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
                GetHaltonSequenceNumber(goNumber, xPrime) * asteroidFieldSize,
                GetHaltonSequenceNumber(goNumber, yPrime) * asteroidFieldSize - asteroidFieldSize / 2,
                1f);

            var gtGO = Instantiate(fuelPrefab, location, Quaternion.identity);
            curGameObjects.Add(gtGO);
            goNumber++;
        }

        for (int i = 0; i < numOxyTanks; i++)
        {
            var location = new Vector3(
                GetHaltonSequenceNumber(goNumber, xPrime) * asteroidFieldSize,
                GetHaltonSequenceNumber(goNumber, yPrime) * asteroidFieldSize - asteroidFieldSize / 2,
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
