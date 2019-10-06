using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Pickupable : MonoBehaviour
{
    [SerializeField] private FloatGameEvent itemPickedUpEvent;

    [SerializeField] private FloatReference pickupAmt;
    private BoxCollider2D _bc;

    // Start is called before the first frame update
    void Start()
    {
        _bc = GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //assume that the only kinematic rigiy boi is the spaceship

        itemPickedUpEvent.Raise(pickupAmt);
        _bc.enabled = false;
    }
}
