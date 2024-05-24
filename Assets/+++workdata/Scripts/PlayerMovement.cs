using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private Rigidbody playerRigidbody;
    private float movingSpeed = 6f;
    [SerializeField] private float sneakingSpeed = 6f;
    [SerializeField] private float defaultSpeed = 6f;
    [SerializeField] private float sprintingSpeed = 6f;
    [SerializeField] private float playerSpeed = 6f;

    private Vector3 inputVector;

    [SerializeField] private float jumpHeight = 5f;

    [Header("Ground Check")]
    [SerializeField] private Transform transformRayStart;
    [SerializeField] private float rayLength = 0.5f;
    [SerializeField] private LayerMask layerGroundCheck;

    [Header("Slope Check")]
    [SerializeField] private float maxAngleSlope = 20f;

    [Header("Camera Control")]
    [SerializeField] Transform transformCameraFollow;
    [SerializeField] float rotateSensivity = 1f;
    private float cameraPitch = 0f;
    private float cameraRoll = 0f;
    [SerializeField] float maxCameraPitch = 60f;
    [SerializeField] bool invertCameraPitch = false;

    [Header("Character Rotation")]
    [SerializeField] private Transform characterBody;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerRigidbody.freezeRotation = true;
        movingSpeed = defaultSpeed;

        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        if (invertCameraPitch)
        {
            cameraPitch = cameraPitch - mouseDelta.y * rotateSensivity;
        }
        else
        {
            cameraPitch = cameraPitch + mouseDelta.y * rotateSensivity;
        }

        cameraPitch = Mathf.Clamp(cameraPitch, -maxCameraPitch, maxCameraPitch);

        cameraRoll = cameraRoll + mouseDelta.x * rotateSensivity;

        transformCameraFollow.localEulerAngles = new Vector3(cameraPitch, cameraRoll, 0);
    }

     private void FixedUpdate()
    {
        if (SlopeCheck())
        {
            Debug.Log("Rotation y CameraFollowObject: " + transformCameraFollow.eulerAngles.y);
            Debug.Log("current inputVector " + inputVector);

            Vector3 movementDirection = new Vector3(inputVector.x * playerSpeed, playerRigidbody.velocity.y, inputVector.z * playerSpeed);

            movementDirection = Quaternion.AngleAxis(transformCameraFollow.eulerAngles.y, Vector3.up) * movementDirection;

            Debug.Log("rotated MovementVector: " + movementDirection);

            playerRigidbody.velocity = movementDirection;
            //playerRigidbody.velocity = new Vector3(inputValue.x * playerSpeed, playerRigidbody.velocity.y, inputValue.z * playerSpeed);

            if (movementDirection != Vector3.zero)
            {
                Vector3 lookdirection = movementDirection;
                lookdirection.y = 0f;
                characterBody.rotation = Quaternion.LookRotation(lookdirection);
            }
            

        }
        else
        {
            inputVector = Vector2.zero;
        }
    }

    private void OnMove(InputValue context)
    {
        inputVector = context.Get<Vector3>();
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
            //Debug.Log(angle);
            if (angle > maxAngleSlope)
            {
                return false;
            }
        }
        return true;
    }
}
