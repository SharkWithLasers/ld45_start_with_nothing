﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource oneShotAudioSource;


    [SerializeField] private AudioClip fuelTankDepletedClip;
    [SerializeField] private AudioClip oxyTankDepletedClip;
    [SerializeField] private AudioClip asteroidHitClip;
    [SerializeField] private AudioClip oxyTankPickedUpClip;
    [SerializeField] private AudioClip fuelTankPickedUpClip;



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
        PlayOneShotClipAlteredPitch(fuelTankDepletedClip);
    }

    public void OnOxyDepleted()
    {
        PlayOneShotClipAlteredPitch(oxyTankDepletedClip);
    }

    public void OnAsteroidHit()
    {
        PlayOneShotClipAlteredPitch(asteroidHitClip);
    }

    public void OnOxyTankPickedUp()
    {
        PlayOneShotClipAlteredPitch(oxyTankPickedUpClip);
    }

    public void OnFuelTankPickedUp()
    {
        PlayOneShotClipAlteredPitch(fuelTankPickedUpClip);
    }

    private void PlayOneShotClipAlteredPitch(AudioClip ac)
    {
        oneShotAudioSource.pitch = Random.Range(0.9f, 1.1f);
        oneShotAudioSource.PlayOneShot(ac);
    }
}
