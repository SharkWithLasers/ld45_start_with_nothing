using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class ShipAnimator : MonoBehaviour
{
    [SerializeField] private Vector2Reference driftVelocity;

    private Option<float> prevHorzNonzeroInput = Option<float>.None;
    private Option<float> prevVertInput = Option<float>.None;

    [SerializeField] private float xRotRange = 30;
    [SerializeField] private float zRotRange = 5;

    private Dictionary<Vector2, Vector3> velocityDirToTargetRotation;


    // Start is called before the first frame update
    void Awake()
    {
        velocityDirToTargetRotation = new Dictionary<Vector2, Vector3>
        {
            { new Vector2(1, 0), new Vector3(0, 0, 0) },
            { new Vector2(-1, 0), new Vector3(0, 180, 0) },
            { new Vector2(1,1), new Vector3(xRotRange, 0, zRotRange) },
            { new Vector2(1,-1), new Vector3(-xRotRange, 0, -zRotRange) },
            { new Vector2(-1,1), new Vector3(-xRotRange, 180, zRotRange) },
            { new Vector2(-1,-1), new Vector3(xRotRange, 180, -zRotRange) },
        };
    }

    // Update is called once per frame
    void Update()
    {
        var horzInput = Input.GetAxisRaw("Horizontal");
        var vertInput = Input.GetAxisRaw("Vertical");


        var horzSign = !Mathf.Approximately(horzInput, 0f)
            ? horzInput
            : prevHorzNonzeroInput.HasValue
                ? prevHorzNonzeroInput.Value
                : 1;

        var vertSign = !Mathf.Approximately(vertInput, 0f)
            ? vertInput
            : prevVertInput.HasValue
                ? prevVertInput.Value
                : 0;
        /*
        var velocitySign = new Vector2(
            Mathf.Approximately(horzInput, 0f) ? prevHorzInput.HasValue ,
            Mathf.Approximately(vertInput, 0f) ? 0f : Mathf.Sign(driftVelocity.Value.y));*/

        var velocitySign = new Vector2(horzSign, vertSign);

        if (velocityDirToTargetRotation.ContainsKey(velocitySign))
        {
            transform.localRotation = Quaternion.Euler(velocityDirToTargetRotation[velocitySign]);
        }
        else
        {
            Debug.Log($"wtf somehow got an unaccounted for velocity dir..{velocitySign}");
        }

        prevHorzNonzeroInput = !Mathf.Approximately(horzInput, 0f) ? horzInput : prevHorzNonzeroInput;
        prevVertInput = vertInput;
    }
}
