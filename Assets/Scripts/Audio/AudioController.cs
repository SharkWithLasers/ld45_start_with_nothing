using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource oneShotAudioSource;


    [SerializeField] private AudioClip fuelTankDepletedClip;
    [SerializeField] private AudioClip oxyTankDepletedClip;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnFuelDepleted()
    {
        oneShotAudioSource.PlayOneShot(fuelTankDepletedClip);
    }

    public void OnOxyDepleted()
    {
        oneShotAudioSource.PlayOneShot(oxyTankDepletedClip);
    }
}
