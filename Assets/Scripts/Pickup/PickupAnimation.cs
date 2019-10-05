using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class PickupAnimation : MonoBehaviour
{
    [SerializeField] private FloatReference pickupRotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * pickupRotationSpeed * Time.deltaTime);
    }
}
