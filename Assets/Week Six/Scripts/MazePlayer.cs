using UnityEngine.XR;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class MazePlayer : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 100f;
    public float smoothTime = 0.1f;

    private Camera playerCamera;
    private Vector2 lookInput;
    private CharacterController characterController;
    private Vector3 smoothMoveVelocity;

    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //player movement
        Vector3 moveDirection = GetMoveDirection();
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        //camera rotation
        Vector2 lookDelta = Mouse.current.delta.ReadValue() * mouseSensitivity * Time.deltaTime;
        lookInput += lookDelta;
        lookInput.y = Mathf.Clamp(lookInput.y, -90f, 90f);
        float targetRotationX = -lookInput.y;
        float targetRotationY = lookInput.x;

        //smooth camera rotate
        Vector3 currentRotation = playerCamera.transform.localEulerAngles;
        float rotationX = Mathf.SmoothDampAngle(currentRotation.x, targetRotationX, ref smoothMoveVelocity.x, smoothTime);
        float rotationY = Mathf.SmoothDampAngle(currentRotation.y, targetRotationY, ref smoothMoveVelocity.y, smoothTime);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);

        //rotate the player object along y-axis
        transform.rotation = Quaternion.Euler(0f, targetRotationY, 0f);
    }

    private Vector3 GetMoveDirection()
    {
        //get forward direction based on camera rotation
        Vector3 forward = playerCamera.transform.forward;        
        //no toppling allowed
        forward.y = 0f;
        forward.Normalize();

        //get right direction
        Vector3 right = playerCamera.transform.right;
        right.y = 0f;
        right.Normalize();

        //cha-cha real smooth
        Vector3 moveDirection = forward * Input.GetAxis("Vertical") + right * Input.GetAxis("Horizontal");
        moveDirection.Normalize();

        return moveDirection;
    }
}
