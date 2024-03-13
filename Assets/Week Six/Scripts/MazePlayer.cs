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
    public int inventoryKeyCount = 0;

    public KeyCode interactKey = KeyCode.E;
    public LayerMask keyLayer;
    public LayerMask doorLayer;

    private Camera playerCamera;
    private Vector2 lookInput;
    private CharacterController characterController;
    private Vector3 smoothMoveVelocity;

    private GameObject currentKey = null;

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

        //interact with objects
        if (Input.GetKeyDown(interactKey))
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {

        Debug.Log("Camera Forward Direction: " + playerCamera.transform.forward);

        int playerLayerMask = 1 << LayerMask.NameToLayer("playerLayer");
        int layerMask = ~playerLayerMask;

        //raycast to check if player is near interactable objects
        RaycastHit hit;
        //max distance to interact with objects
        //float maxInteractDistance = 10f;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, Mathf.Infinity, layerMask))
        {

            //debug
            Debug.Log("Raycast hit Point: " + hit.point);
            Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
            Debug.DrawLine(playerCamera.transform.position, hit.point, Color.red, 0.5f);

            //check if the hit object is a key
            if (((1 << hit.collider.gameObject.layer) & keyLayer) != 0)
            {
                Debug.Log("Key found!");
                PickUpKey(hit.collider.gameObject);
            }
            //check if the hit object is a door
            else if (((1 << hit.collider.gameObject.layer) & doorLayer) != 0)
            {
                Debug.Log("Door found!");
                TryOpenDoor(hit.collider.gameObject);
            }
        }
        else
        {
            Debug.Log("Nothing to interact with!");
        }
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

    private void PickUpKey(GameObject keyObject)
    {
        Debug.Log("Key picked up!");
        //add key to inventory
        currentKey = keyObject;
        inventoryKeyCount++;
        //hide key
        keyObject.SetActive(false);
    }

    private void TryOpenDoor(GameObject doorObject)
    {
        //check if player has key
        if (currentKey != null)
        {
            //remove key from inventroy
            Destroy(currentKey);

            //open door
            DoorHandler doorHandler = doorObject.GetComponent<DoorHandler>();
            doorHandler.OpenDoor();
            Debug.Log("Door opened!");

            //reset current key
            currentKey = null;
        }
        else
        {
            Debug.Log("You need a key to open this door!");
        }

    }

}
