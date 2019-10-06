using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraumatibleCamera : MonoBehaviour
{

    public float maxAngle = 0.5f;
    public Vector3 maxOffset = Vector3.zero;
    public const float maxTranslationRatio = 0.005f;

    public float traumaMax = 1.0f;
    public float traumaMin = 0.0f;

    public float traumitizeAddition = 0.75f;
    public float traumaReducer = 1f;

    private float _trauma;
    private Vector3 _basePosition;
    private Quaternion _baseRotation;

    private float _shake => Mathf.Pow(_trauma, 2);

    private void Awake()
    {
        _basePosition = transform.position;
        _baseRotation = transform.rotation;
        _trauma = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        var degreesToRotate = maxAngle * _shake * Random.Range(-1f, 1f);
        transform.rotation = _baseRotation * Quaternion.Euler(Vector3.forward * degreesToRotate);
        /*
        var offsetX = maxOffset.x * _shake * Random.Range(-1f, 1f);
        var offsetY = maxOffset.y * _shake * Random.Range(-1f, 1f);
        var offsetZ = maxOffset.z * _shake * Random.Range(-1f, 1f);


        transform.position = new Vector3(
            _basePosition.x + offsetX,
            _basePosition.y + offsetY,
            _basePosition.z + offsetZ);*/

        _trauma = Mathf.Max(traumaMin, _trauma - traumaReducer * Time.deltaTime);
    }

    public void Traumatize()
    {
        _trauma = Mathf.Min(_trauma + traumitizeAddition, traumaMax);
    }
}