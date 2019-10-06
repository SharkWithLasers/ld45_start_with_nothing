using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class OxygenConsumer : MonoBehaviour
{
    [SerializeField] private FloatReference oxygenLeft;
    [SerializeField] private FloatReference maxOxygen;

    [SerializeField] private GameEvent OxygenDepletedEvent;

    [SerializeField] private float initialOxygen = 30f;
    private bool shouldBeDepleting = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldBeDepleting)
        {
            oxygenLeft.Value = Mathf.Max(0f, oxygenLeft.Value - Time.deltaTime);

            if (Mathf.Approximately(oxygenLeft.Value, 0f))
            {

                shouldBeDepleting = false;
                OxygenDepletedEvent.Raise();
            }
        }
    }

    public void OnOxygenPickedUp(float amt)
    {
        oxygenLeft.Value  = Mathf.Min(maxOxygen, oxygenLeft.Value +  amt);
    }

    public void OnLevelEnded()
    {
        shouldBeDepleting = false;
    }

    public void OnLevelStart()
    {
        shouldBeDepleting = true;
        oxygenLeft.Value = initialOxygen;
    }
}
