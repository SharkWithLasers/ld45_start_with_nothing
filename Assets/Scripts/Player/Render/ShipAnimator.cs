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
    [SerializeField] private float pureVertZRotRatio = 4;

    [SerializeField] private ParticleSystem deathPS;
    [SerializeField] private GameObject shipModel;


    private Dictionary<Vector2, Vector3> velocityDirToTargetRotation;
    private bool levelRunning;


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
        if (!levelRunning)
        {
            return;
        }

        var horzInput = Input.GetAxisRaw("Horizontal");
        var vertInput = Input.GetAxisRaw("Vertical");

        var hasHorzInput = !Mathf.Approximately(horzInput, 0f);

        var horzSign = hasHorzInput
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
            var rotationToUse = velocityDirToTargetRotation[velocitySign];
            if (!hasHorzInput)
            {
                rotationToUse.z *= pureVertZRotRatio;
            }


            transform.localRotation = Quaternion.Euler(rotationToUse);
        }
        else
        {
            Debug.Log($"wtf somehow got an unaccounted for velocity dir..{velocitySign}");
        }

        prevHorzNonzeroInput = !Mathf.Approximately(horzInput, 0f) ? horzInput : prevHorzNonzeroInput;
        prevVertInput = vertInput;
    }

    public void OnLevelEnded()
    {
        levelRunning = false;
    }

    public void OnAsteroidHit()
    {

        levelRunning = false;
        shipModel.SetActive(false);

        deathPS.Play();


    }

    public void OnLevelStarted()
    {
        shipModel.SetActive(true);

        levelRunning = true;
        prevHorzNonzeroInput = Option<float>.None;
        prevVertInput = Option<float>.None;
    }
}
