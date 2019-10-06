using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Pickupable : MonoBehaviour
{
    [SerializeField] private FloatGameEvent itemPickedUpEvent;

    [SerializeField] private FloatReference pickupAmt;

    [SerializeField] private GameObject pickupTextPrefab;

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
        Instantiate(pickupTextPrefab, transform.position, Quaternion.identity);

        itemPickedUpEvent.Raise(pickupAmt);
        gameObject.SetActive(false);    
        //_bc.enabled = false;
    }
}
