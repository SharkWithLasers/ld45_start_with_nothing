using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private LevelGenerator levelGen;

    [SerializeField] private TutorialDirector td;

    //ugh
    [SerializeField] private GameObject player;
    [SerializeField] private MinimapCamera mmCamera;


    [SerializeField] private GameEvent LevelStartedEvent;
    [SerializeField] private GameEvent LevelWonEvent;
    [SerializeField] private GameEvent LevelLostEvent;

    [SerializeField] private GameEvent TutorialStartedEvent;


    [SerializeField] private GameEvent PlayerSelectEvent;

    private enum LevelState
    {
        IntroSection,
        TutorialSection,
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
        curState = LevelState.IntroSection;

        //StartLevel(Option<bool>.None, isTutorialLevel: true);
    }

    // Start is called before the first frame update
    void StartLevel(Option<bool> shouldBeEasier, bool isTutorialLevel)
    {
        curState = LevelState.LevelLoading;

        levelGen.GenerateLevel(player, mmCamera, shouldBeEasier, isTutorialLevel);

        //set player to proper place (perhaps this should be in player script lol, or levelGen?)
        
        if (!isTutorialLevel)
        {
            curState = LevelState.LevelStarted;
            LevelStartedEvent.Raise();
        }

        //StartCoroutine(ChillThenGenLevel(shouldBeEasier));
    }

    void StartNextLevel(bool shouldBeEasier)
    {
        // fadeout animation

        // generate level
        StartLevel(shouldBeEasier, isTutorialLevel: false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && curState == LevelState.IntroSection)
        {
            PlayerSelectEvent.Raise();

            td.IntroClicked();

            curState = LevelState.TutorialSection;

            //StartLevel(Option<bool>.None, isTutorialLevel: false);
            /*
            PlayerSelectEvent.Raise();
            //StartNextLevel(shouldBeEasier: false);
            return;*/
        }

        if (Input.GetKeyDown(KeyCode.R) && curState == LevelState.LevelLost)
        {
            PlayerSelectEvent.Raise();
            StartNextLevel(shouldBeEasier: true);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && curState == LevelState.LevelWon)
        {
            PlayerSelectEvent.Raise();
            StartNextLevel(shouldBeEasier: false);
            return;
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

    public void OnTutorialPartOneFinished()
    {
        StartLevel(Option<bool>.None, isTutorialLevel: true);

        td.TutLevelLoaded();
    }

    public void OnTutorialPartTwoFinished()
    {
        curState = LevelState.LevelStarted;
        LevelStartedEvent.Raise();
    }
}
