using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonController : MonoBehaviour
{
    public float walkSpeed = 5.0f;
    public float runSpeed = 10.0f;
    public float jumpForce = 8.0f;
    public float sensitivity = 2.0f;
    public float shakeIntensity = 0.05f;
    public float shakeSpeed = 5.0f;

    private Rigidbody rb;
    private float verticalRotation = 0f;
    private bool isRunning = false;
    private bool isShiftKeyDown = false;
    private bool isGrounded;
    private float originalPosY;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Freeze rotation so we only rotate using mouse input
        originalPosY = Camera.main.transform.localPosition.y;
        // Lock cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Rotation
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = -Input.GetAxis("Mouse Y") * sensitivity;

        verticalRotation += mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        transform.Rotate(0, mouseX, 0);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        // Movement
        float moveSpeed = isRunning ? runSpeed : walkSpeed;
        float forwardSpeed = Input.GetAxis("Vertical") * moveSpeed;
        float sideSpeed = Input.GetAxis("Horizontal") * moveSpeed;

        Vector3 moveDirection = (transform.forward * forwardSpeed + transform.right * sideSpeed).normalized;
        moveDirection *= moveSpeed; // Apply speed here

        // Check if grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f); // Adjust the raycast length as needed

        if (isGrounded)
        {
            // Jumping
            if (Input.GetButtonDown("Jump"))
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }

        // Apply movement
        rb.AddForce(moveDirection);

        // Toggle running
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isShiftKeyDown = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isShiftKeyDown = false;
        }

        isRunning = isShiftKeyDown && isGrounded;

        // Camera shaking when running
        if (isRunning)
        {
            float shakeAmount = Mathf.Sin(Time.time * shakeSpeed) * shakeIntensity;
            Camera.main.transform.localPosition = new Vector3(Camera.main.transform.localPosition.x,
                originalPosY + shakeAmount,
                Camera.main.transform.localPosition.z);
        }
        else
        {
            // Reset camera position
            Camera.main.transform.localPosition = new Vector3(Camera.main.transform.localPosition.x,
                originalPosY,
                Camera.main.transform.localPosition.z);
        }
    }
}
