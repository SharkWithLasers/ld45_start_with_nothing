using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class OxygenConsumer : MonoBehaviour
{
    [SerializeField] private FloatReference oxygenLeft;
    [SerializeField] private FloatReference maxOxygen;

    [SerializeField] private float initialOxygen = 30f;

    // Start is called before the first frame update
    void Start()
    {
        oxygenLeft.Value = initialOxygen;
    }

    // Update is called once per frame
    void Update()
    {
        oxygenLeft.Value = Mathf.Max(0f, oxygenLeft.Value - Time.deltaTime);

        if (Mathf.Approximately(oxygenLeft.Value, 0f))
        {
            Debug.Log("ran outta oxy yo");
        }
    }

    public void OnOxygenPickedUp(float amt)
    {
        oxygenLeft.Value  = Mathf.Min(maxOxygen, oxygenLeft.Value +  amt);
    }
}
