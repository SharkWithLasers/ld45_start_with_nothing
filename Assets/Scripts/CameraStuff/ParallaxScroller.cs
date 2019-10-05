using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class ParallaxScroller : MonoBehaviour
{
    // TODO, if I decide to make this an exploration game thingy...use current velocity of ship
    [SerializeField] private FloatReference focalPointSpeed;

    [SerializeField] private Vector2Reference currentDriftVelocity;
    [SerializeField] private Vector3Reference cameraSmoothVelocity;

    [SerializeField] private FloatReference parallaxRatio;

    private MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        var texOffset = meshRenderer.material.mainTextureOffset;
        texOffset += (currentDriftVelocity.Value + (Vector2) cameraSmoothVelocity.Value)
            * Time.deltaTime
            * parallaxRatio;
        //texOffset.x += Time.deltaTime * (focalPointSpeed * parallaxRatio);

        meshRenderer.material.mainTextureOffset = texOffset;
    }
}
