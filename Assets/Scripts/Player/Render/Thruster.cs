using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    [SerializeField] private AudioSource thrusterAS;
    [SerializeField] private ParticleSystem thrusterPS;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PeriodicallyFuckWithThrusterPitch());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TryStartThrusting()
    {
        if (!thrusterPS.isPlaying)
        {
            thrusterPS.Play();
        }

        if (!thrusterAS.isPlaying)
        {
            thrusterAS.Play();
        }
    }

    public void TryStopThrusting()
    {
        if (thrusterPS.isPlaying)
        {
            thrusterPS.Stop();
        }

        if (thrusterAS.isPlaying)
        {
            thrusterAS.Stop();
        }
    }

    public void OnLevelEnded()
    {
        TryStopThrusting();
    }

    IEnumerator PeriodicallyFuckWithThrusterPitch()
    {
        while (true)
        {
            thrusterAS.pitch = Random.Range(0.8f, 1.2f);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
