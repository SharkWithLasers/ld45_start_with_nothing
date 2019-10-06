﻿using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private LevelGenerator levelGen;

    //ugh
    [SerializeField] private GameObject player;

    [SerializeField] private GameEvent LevelStartedEvent;
    [SerializeField] private GameEvent LevelWonEvent;
    [SerializeField] private GameEvent LevelLostEvent;

    private enum LevelState
    {
        IntroScreen,
        LevelLoading,
        LevelStarted,
        LevelLost,
        LevelWon
    }

    private LevelState curState;

    //ugh I know
    public enum LevelLostReason
    {
        OutOfOxygen,
        HitAnAsteroid,
    }

    public Option<LevelLostReason> levelLostReason = Option<LevelLostReason>.None;

    private void Start()
    {
        StartLevel();
    }

    // Start is called before the first frame update
    void StartLevel(bool shouldBeEasier = false)
    {
        levelGen.GenerateLevel(player);

        //set player to proper place (perhaps this should be in player script lol, or levelGen?)

        curState = LevelState.LevelStarted;

        LevelStartedEvent.Raise();
    }

    void StartNewLevel(bool shouldBeEasier = false)
    {
        // fadeout animation

        // generate level
        StartLevel(shouldBeEasier);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && curState == LevelState.LevelLost)
        {
            StartNewLevel(shouldBeEasier: true);
        }

        if (Input.GetKeyDown(KeyCode.Space) && curState == LevelState.LevelWon)
        {
            StartNewLevel(shouldBeEasier: false);
        }
    }


    public void OnPlanetReached()
    {
        curState = LevelState.LevelWon;

        LevelWonEvent.Raise();
    }

    public void OnAsteroidHit()
    {
        levelLostReason = LevelLostReason.HitAnAsteroid;
        curState = LevelState.LevelLost;

        LevelLostEvent.Raise();
    }

    public void OnOxygenDepleted()
    {
        levelLostReason = LevelLostReason.OutOfOxygen;
        curState = LevelState.LevelLost;
        LevelLostEvent.Raise();
    }
}
