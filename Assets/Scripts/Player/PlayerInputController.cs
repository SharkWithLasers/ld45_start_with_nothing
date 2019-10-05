using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private FloatReference moveSpeed;
    private MovementController movementController;


    // Start is called before the first frame update
    void Start()
    {
        movementController = GetComponent<MovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        var horzInput = Input.GetAxisRaw("Horizontal");
        var vertInput = Input.GetAxisRaw("Vertical");

        if (horzInput == 0f && vertInput == 0f)
        {
            return;
        }

        var displacement = new Vector2(
            horzInput,
            vertInput).normalized * moveSpeed * Time.deltaTime;

        movementController.TryDisplace(displacement, transform);
    }
}
