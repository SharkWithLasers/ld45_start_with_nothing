using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;


//seems like this and pickupable are more like...collidable
[RequireComponent(typeof(BoxCollider2D))]
public class Asteroid : MonoBehaviour
{
    [SerializeField] private GameEvent asteroidCollisionEvent;

    private BoxCollider2D _bc;

    // Start is called before the first frame update
    void Start()
    {
        _bc = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        asteroidCollisionEvent.Raise();
        Debug.Log("asteroid hit yo");
        //_bc.enabled = false;
    }
}
