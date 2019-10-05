using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Planet : MonoBehaviour
{
    [SerializeField] private GameEvent planetReachedGameEvent;
    private BoxCollider2D _bc;

    public void Start()
    {
        _bc = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // assumption: only rigiyboi is spaceship
        planetReachedGameEvent.Raise();

        _bc.enabled = false;
    }
}
