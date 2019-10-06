using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    private Camera _camera;

    // Start is called before the first frame update
    void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupMinimapSizing(float mainWorldSizeInUnits)
    {
        transform.position = new Vector3(
            mainWorldSizeInUnits / 2,
            0f,
            transform.position.z);

        _camera.orthographicSize = mainWorldSizeInUnits / 2;
    }

}
