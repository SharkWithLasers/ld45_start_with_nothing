using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class DampenedFollowCam : MonoBehaviour
{
    [SerializeField] private Transform objectToFollow;
    [SerializeField] private Vector2Reference objectVelocity;

    [SerializeField] private FloatReference minDriftSpeed;
    [SerializeField] private FloatReference maxDriftSpeed;

    [SerializeField] private float minDisplacementScale = 0.25f;
    [SerializeField] private float maxDisplacementScale = 0.7f;
    [SerializeField] private float smoothTime = 1f;

    [SerializeField] Vector3Reference _smoothVelocity;

    // Start is called before the first frame update
    void Start()
    {
        _smoothVelocity.Value = Vector3.zero;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        var objectCurSpeed = objectVelocity.Value.magnitude;
        var objectVelocityDir = objectVelocity.Value.normalized;

        var displacementScale = MathUtil.GetNormalizedValue(objectCurSpeed, minDriftSpeed, maxDriftSpeed,
            minDisplacementScale, maxDisplacementScale);

        //assumption is that we are children of the bodyToFollow
        var targetLocalPosition = new Vector3(
            objectVelocityDir.x * displacementScale * (9.6f / 2),
            objectVelocityDir.y * displacementScale * (6 / 2),
            transform.localPosition.z);

        var smoothVelocityVal = _smoothVelocity.Value;

        transform.localPosition = Vector3.SmoothDamp(
            transform.localPosition, targetLocalPosition, ref smoothVelocityVal, smoothTime);

        _smoothVelocity.Value = smoothVelocityVal;
    }
}
