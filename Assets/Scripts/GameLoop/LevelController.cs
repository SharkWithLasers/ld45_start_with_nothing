using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private LevelGenerator levelGen;

    [SerializeField] private GameEvent LevelWonEvent;
    [SerializeField] private GameEvent LevelLostEvent;

    //ugh I know
    public enum LevelLostReason
    {
        OutOfOxygen,
        HitAnAsteroid,
    }

    public Option<LevelLostReason> levelLostReason = Option<LevelLostReason>.None;

    // Start is called before the first frame update
    void Start()
    {
        levelGen.GenerateLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnPlanetReached()
    {
        LevelWonEvent.Raise();
    }

    public void OnAsteroidHit()
    {
        levelLostReason = LevelLostReason.HitAnAsteroid;
        LevelLostEvent.Raise();
    }

    public void OnOxygenDepleted()
    {
        levelLostReason = LevelLostReason.OutOfOxygen;

        LevelLostEvent.Raise();
    }
}
