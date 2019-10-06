using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class TutorialDirector : MonoBehaviour
{
    [SerializeField] private Thruster thruster;

    [SerializeField] private UIController uiController;

    [SerializeField] private GameEvent tutorialTextEvent;

    [SerializeField] private GameEvent tutorialPartOneFinishedEvent;
    [SerializeField] private GameEvent tutorialPartTwoFinishedEvent;

    [SerializeField] private FloatReference fuelLeft;
    [SerializeField] private FloatReference oxygenLeft;

    private enum TutState
    {
        NotStarted,
        IntroClicked,
        MPrompted,
        MapToggled
    };

    private TutState ts;
    // Start is called before the first frame update


    void Start()
    {
        ts = TutState.NotStarted;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && ts == TutState.MPrompted)
        {
            ts = TutState.MapToggled;
        }
    }

    public void IntroClicked()
    {
        StartCoroutine(RightBeforeTutorialGen());
    }

    IEnumerator RightBeforeTutorialGen()
    {
        uiController.FadeIntroText();

        yield return new WaitForSeconds(1f);

        fuelLeft.Value = 0.5f;
        StartCoroutine(thruster.ThrusterSputsOut());

        yield return new WaitForSeconds(3.25f);

        // Oh no! we've run out of fuel!
        uiController.ShowTutorialFuelText();
        tutorialTextEvent.Raise();

        yield return new WaitForSeconds(2.8f);

        // And we are going to run out of oxygen!
        oxygenLeft.Value = 32.5f;
        uiController.ShowTutorialOxygenText();
        tutorialTextEvent.Raise();

        yield return new WaitForSeconds(2.8f);

        uiController.ShowTutorialWeMustGetBackToOurHomePlanetText();
        tutorialTextEvent.Raise();

        yield return new WaitForSeconds(2.8f);

        uiController.ShowTutorialPressMToToggleMapText();
        tutorialTextEvent.Raise();
        ts = TutState.MPrompted;

        //yield return new WaitForSeconds(2.8f);

        // Press 'M' to view the map

        yield return new WaitUntil(() => ts == TutState.MapToggled);

        Debug.Log("ayy baybayboobe");

        tutorialPartOneFinishedEvent.Raise();

        // start tutorial level ...
        // 
    }

    public void TutLevelLoaded()
    {
        StartCoroutine(RightAfterTutGen());
        //StartCoroutine(RightBeforeTutorialGen());
    }

    IEnumerator RightAfterTutGen()
    {
        uiController.ShowMinimapForTut();

        yield return new WaitForSeconds(2.8f);

        uiController.ShowTutText("Our Spaceship is in blue");
        tutorialTextEvent.Raise();

        yield return new WaitForSeconds(2.8f);

        uiController.ShowTutText("Our planet is in red");
        tutorialTextEvent.Raise();

        yield return new WaitForSeconds(2.8f);

        uiController.ShowTutText("we'll need fuel and oxygen on the way");
        tutorialTextEvent.Raise();

        yield return new WaitForSeconds(2.8f);

        uiController.ShowTutText("If we find fuel, use 'WASD+J' to thrust");
        tutorialTextEvent.Raise();

        yield return new WaitForSeconds(2.8f);

        uiController.ShowTutText("Oh yeah, and watch out for uncharted asteroids");
        tutorialTextEvent.Raise();

        yield return new WaitForSeconds(2.8f);

        uiController.ShowTutText("Good luck, we are all counting on you");
        tutorialTextEvent.Raise();

        yield return new WaitForSeconds(2.8f);

        tutorialPartTwoFinishedEvent.Raise();
        tutorialTextEvent.Raise();
    }
    /*
    public void OnTutorialStarted()
    {
        // thruster has to thrust then stop thrusting
        thruster.DepleteFuel();

        StartCoroutine(Tutorial())

        // then we can play some messages... in the canvas?

        // Oh no! we ran out of fuel!

        // 
    }

    IEnumerator Tutorial()
    {
        StartCoroutine(thruster.DepleteFuel());
        //yield return thrust
    }*/
}
