using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody playerRigidbody;
    private float movingSpeed = 6f;
    [SerializeField] private float sneakingSpeed = 6f;
    [SerializeField] private float defaultSpeed = 6f;
    [SerializeField] private float sprintingSpeed = 6f;
    [SerializeField] private float playerSpeed = 6f;

    private Vector3 inputValue;

    [SerializeField] private float jumpHeight = 5f;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        movingSpeed = defaultSpeed;
    }

     private void FixedUpdate()
    {
        playerRigidbody.velocity = new Vector3(inputValue.x * playerSpeed, playerRigidbody.velocity.y, inputValue.z * playerSpeed);
    }

    private void OnMove(InputValue context)
    {
        inputValue = context.Get<Vector3>();
    }

    private void OnJump()
    {
        playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, jumpHeight, playerRigidbody.velocity.z);
    }

    void OnSneak(InputValue inputVal)
    {
        float isSneaking = inputVal.Get<float>();
        Debug.Log(isSneaking);

        //round the float number (Mathf.RoundToInt) to the nearest integer, so that a direct comparison (==)
        if (Mathf.RoundToInt(isSneaking) == 1)
        {
            playerSpeed = sneakingSpeed;
        }
        else
        {
            playerSpeed = defaultSpeed;
        }
    }

    void OnSprint(InputValue inputVal)
    {
        float isSprinting = inputVal.Get<float>();
        Debug.Log(isSprinting);

        if (Mathf.RoundToInt(isSprinting) == 1)
        {
            playerSpeed = sprintingSpeed;
        }
        else
        {
            playerSpeed = defaultSpeed;
        }
    }
}
