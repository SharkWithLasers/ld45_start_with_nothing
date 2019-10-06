using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIController : MonoBehaviour
{
    [SerializeField] private LevelController levelController;

    [SerializeField] private GameObject minimapCamera;
    [SerializeField] private GameObject minimapUI;

    [SerializeField] private TextMeshProUGUI levelLostText;
    [SerializeField] private GameObject levelLostUI;

    [SerializeField] private GameObject levelWonUI;


    private bool minimapOn = false;
    private bool canInteract;

    // Start is called before the first frame update
    void Start()
    {
        InitialLevelUI();
    }

    void InitialLevelUI()
    {
        minimapOn = false;
        SetMinimapTo(minimapOn);
        levelWonUI.SetActive(false);
        SetLevelLostUITo(false);
        canInteract = true;
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
}
