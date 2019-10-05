using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
	[SerializeField] private Camera orthoCamera;
    private Vector2 boundsLowerLeft;
    private Vector2 boundsUpperRight;

    //pixels-per-unit assumed to be 100 right now
    //private float pixelsPerUnit = 100;
    // Start is called before the first frame update
    void Start()
    {
        var cameraHalfSize = orthoCamera.orthographicSize;
        var pixelsPerUnit = Screen.height / (cameraHalfSize * 2);

        var horzHalfSize = Screen.width / (2 *  pixelsPerUnit);

        boundsLowerLeft = new Vector2(-horzHalfSize, -cameraHalfSize);
        boundsUpperRight = new Vector2(horzHalfSize, cameraHalfSize);

        //bounds.
        //var camera
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TryDisplace(Vector2 unitsToDisplace, Transform playerToMove)
    {
        var playerPosition = new Vector3(
            Mathf.Clamp(unitsToDisplace.x + playerToMove.position.x, boundsLowerLeft.x, boundsUpperRight.x),
            Mathf.Clamp(unitsToDisplace.y + playerToMove.position.y, boundsLowerLeft.y, boundsUpperRight.y),
            playerToMove.position.z);

		playerToMove.position = playerPosition;
        //playerToMove.tra

    }


}
