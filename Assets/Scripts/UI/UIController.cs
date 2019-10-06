using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    [SerializeField] private LevelController levelController;

    [SerializeField] private GameObject minimapCamera;
    [SerializeField] private GameObject minimapUI;

    [SerializeField] private TextMeshProUGUI levelLostText;
    [SerializeField] private GameObject levelLostUI;

    [SerializeField] private GameObject levelWonUI;
    [SerializeField] private GameObject fuelGauge;
    [SerializeField] private GameObject oxygenGauge;




    [SerializeField] private GameObject introScreenUI;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI instructionsText;

    [SerializeField] private TextMeshProUGUI tutorialText;


    private bool minimapOn = false;
    private bool canInteract;

    // Start is called before the first frame update
    void Start()
    {
        IntroScreenUI();
    }

    void IntroScreenUI()
    {
        introScreenUI.SetActive(true);

        tutorialText.enabled = false;


        minimapOn = false;
        SetMinimapTo(minimapOn);
        levelWonUI.SetActive(false);
        SetLevelLostUITo(false);
        canInteract = false;
        fuelGauge.SetActive(false);
        oxygenGauge.SetActive(false);
    }

    void InitialLevelUI()
    {
        introScreenUI.SetActive(false);

        tutorialText.enabled = false;

        minimapOn = false;
        SetMinimapTo(minimapOn);
        levelWonUI.SetActive(false);
        SetLevelLostUITo(false);
        canInteract = true;

        fuelGauge.SetActive(true);
        oxygenGauge.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!canInteract)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            SetMinimapTo(!minimapOn);
        }
    }

    void SetMinimapTo(bool val)
    {
        minimapCamera.SetActive(val);
        minimapUI.SetActive(val);
        minimapOn = val;
    }

    public void OnLevelWon()
    {
        SetMinimapTo(false);
        SetLevelLostUITo(false);

        levelWonUI.SetActive(true);
        canInteract = false;
    }

    public void OnLevelLost()
    {
        SetMinimapTo(false);
        levelWonUI.SetActive(false);

        SetLevelLostUITo(true);
        canInteract = false;
    }

    public void OnLevelStarted()
    {
        InitialLevelUI();
    }    

    void SetLevelLostUITo(bool val)
    {
        if (val)
        {
            var levelLostReasonText = !levelController.levelLostReason.HasValue
                ? "Level Failed :("
                : levelController.levelLostReason.Value == LevelController.LevelLostReason.HitAnAsteroid
                    ? "Hit An Asteroid :("
                    : "Oxygen Depleted :(";

            levelLostText.text = $"{levelLostReasonText}\n\nPress 'R' to Retry";
        }

        levelLostUI.SetActive(val);
    }

    public void FadeIntroText()
    {
        titleText.DOFade(0, 0.75f);
        instructionsText.DOFade(0, 0.75f);
    }

    public void ShowTutorialFuelText()
    {
        tutorialText.enabled = true;
        tutorialText.text = "Oh no! we've run out of fuel!";
        fuelGauge.SetActive(true);
    }

    public void ShowTutorialOxygenText()
    {
        tutorialText.enabled = true;
        tutorialText.text = "And we'll soon run out of oxygen!";
        oxygenGauge.SetActive(true);
    }

    public void ShowTutorialWeMustGetBackToOurHomePlanetText()
    {
        tutorialText.enabled = true;
        tutorialText.text = "We must somehow get back to our home planet";
    }

    public void ShowTutorialPressMToToggleMapText()
    {
        tutorialText.enabled = true;
        tutorialText.text = "Press 'M' to toggle the map";
    }

    public void ShowMinimapForTut()
    {
        SetMinimapTo(true);
    }

    public void ShowTutText(string text)
    {
        tutorialText.enabled = true;
        tutorialText.text = text;
    }
}
