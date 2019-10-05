using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class PickupAnimation : MonoBehaviour
{
    [SerializeField] private FloatReference pickupRotationSpeed;
    [SerializeField] private Vector3 rotateAround = Vector3.up;

    [SerializeField] private bool useRandomRotation = true;

    // Start is called before the first frame update
    void Start()
    {
        rotateAround = useRandomRotation
            ? new Vector3(Random.value, Random.value, Random.value).normalized
            : rotateAround;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotateAround * pickupRotationSpeed * Time.deltaTime);
    }
}
