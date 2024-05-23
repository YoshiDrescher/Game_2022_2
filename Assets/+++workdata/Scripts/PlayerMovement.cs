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
    private Vector2 inputVector;

    [SerializeField] private float jumpHeight = 5f;

    [SerializeField] private Transform transformRayStart;
    [SerializeField] private float rayLength = 0.5f;
    [SerializeField] private LayerMask layerGroundCheck;

    [SerializeField] private float maxAngleSlope = 20f;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        movingSpeed = defaultSpeed;
    }

     private void FixedUpdate()
    {
        if (SlopeCheck())
        {
            playerRigidbody.velocity = new Vector3(inputValue.x * playerSpeed, playerRigidbody.velocity.y, inputValue.z * playerSpeed);
        }
        else
        {
            inputVector = Vector2.zero;
        }
    }

    private void OnMove(InputValue context)
    {
        inputValue = context.Get<Vector3>();
    }

    private void OnJump()
    {
        if (GroundCheck())
        {
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, jumpHeight, playerRigidbody.velocity.z);
        }
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

    bool GroundCheck()
    {
        return Physics.Raycast(transformRayStart.position, Vector3.down, rayLength, layerGroundCheck);
    }

    bool SlopeCheck()
    {
        RaycastHit hit;

        Physics.Raycast(transformRayStart.position, Vector3.down, out hit, rayLength, layerGroundCheck);

        if (hit.collider != null)
        {
            float angle = Vector3.Angle(Vector3.up, hit.normal);
            Debug.Log(angle);
            if (angle > maxAngleSlope)
            {
                return false;
            }
        }
        return true;
    }
}
