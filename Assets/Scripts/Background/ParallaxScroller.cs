using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class ParallaxScroller : MonoBehaviour
{
    // TODO, if I decide to make this an exploration game thingy...use current velocity of ship
    [SerializeField] private FloatReference focalPointSpeed;

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

        texOffset.x += Time.deltaTime * (focalPointSpeed * parallaxRatio);

        meshRenderer.material.mainTextureOffset = texOffset;
    }
}
