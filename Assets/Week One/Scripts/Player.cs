using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public float maxVerticalLook = 90f;
    public float minVerticalLook = -90f;
    public float jumpSpeed;

    private float ySpeed;


    private Rigidbody rb;
    private float rotationX = 0f;

    private void Start()
    {

        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.freezeRotation = true;
        }
        else
        {
            Debug.LogError("Rigidbody not found.");
        }
    }

    private void Update()
    {
        //player movement using keyboard input
        HandleMovement();

        //player rotation using mouse input
        HandleMouseLook();

    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;
        
        float magnitude = movement.magnitude;
        movement.x = Mathf.Clamp01(magnitude);
        transform.Translate(movement * magnitude * moveSpeed * Time.deltaTime, Space.World);

        ySpeed += Physics.gravity.y * Time.deltaTime;
        if (Input.GetButtonDown("Jump"))
        {

            ySpeed = -0.5f;

        }

        Vector3 vel = movement * magnitude;
        vel.y = ySpeed;
        transform.Translate(vel * Time.deltaTime);

        //transform movement based on player rotation
        movement = transform.TransformDirection(movement);

        if (rb != null)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
        }
        else
        {
            Debug.LogError("Rigidbody not found.");
        }
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;

        //rotate player based on mouse movement
        transform.Rotate(Vector3.up * mouseX);

        //get mouse Y
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        //rotate camera based on mouse Y
        rotationX -= mouseY;
        //limit how far the camera can look up and down
        rotationX = Mathf.Clamp(rotationX, minVerticalLook, maxVerticalLook);

        //rotate cemera based on mouse X
        Camera.main.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }
}