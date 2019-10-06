using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject minimapCamera;
    [SerializeField] private GameObject minimapUI;

    private bool minimapOn = false;

    // Start is called before the first frame update
    void Start()
    {
        minimapOn = false;
        SetMinimapTo(minimapOn);
    }

    // Update is called once per frame
    void Update()
    {
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
}
