using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private LevelGenerator levelGen;

    //ugh
    [SerializeField] private GameObject player;
    [SerializeField] private MinimapCamera mmCamera;


    [SerializeField] private GameEvent LevelStartedEvent;
    [SerializeField] private GameEvent LevelWonEvent;
    [SerializeField] private GameEvent LevelLostEvent;

    [SerializeField] private GameEvent PlayerSelectEvent;

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
        StartLevel(Option<bool>.None);
    }

    // Start is called before the first frame update
    void StartLevel(Option<bool> shouldBeEasier)
    {
        curState = LevelState.LevelLoading;

        StartCoroutine(ChillThenGenLevel(shouldBeEasier));
    }

    IEnumerator ChillThenGenLevel(Option<bool> shouldBeEasier)
    {
        yield return new WaitForSeconds(0.33f);

        levelGen.GenerateLevel(player, mmCamera, shouldBeEasier);

        //set player to proper place (perhaps this should be in player script lol, or levelGen?)

        curState = LevelState.LevelStarted;

        LevelStartedEvent.Raise();
    }

    void StartNextLevel(bool shouldBeEasier = false)
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
            PlayerSelectEvent.Raise();
            StartNextLevel(shouldBeEasier: true);
        }

        if (Input.GetKeyDown(KeyCode.Space) && curState == LevelState.LevelWon)
        {
            PlayerSelectEvent.Raise();
            StartNextLevel(shouldBeEasier: false);
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
