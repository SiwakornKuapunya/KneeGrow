using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Movement speed
    public float jumpForce = 10f; // Jump force
    public Transform groundCheck; // Transform representing the position of the ground check
    public LayerMask groundLayer; // Layermask for identifying ground

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, 0.1f, groundLayer);

        // Player movement
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 moveDirection = new Vector3(horizontalInput, 0, 0) * speed * Time.deltaTime;
        transform.Translate(moveDirection);

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}