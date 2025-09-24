using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;            // horizontal movement speed
    public float jumpForce = 7f;            // jump velocity
    public float minX = -1f;                // left boundary
    public float maxX = 1f;                 // right boundary
    public LayerMask groundLayer;           // assign Ground layer

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool isCrouching = false;
    public float crouchSpeedMultiplier = 0.5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 3f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        // Ground check using OverlapBox
        isGrounded = Physics2D.OverlapBox(transform.position + Vector3.down * 0.6f,
                                           new Vector2(0.5f, 0.1f), 0f, groundLayer);

        // Crouching
        isCrouching = Keyboard.current.downArrowKey.isPressed;

        // Small horizontal input for illusion
        float moveInput = 0f;
        if (Keyboard.current.leftArrowKey.isPressed) moveInput = -1f;
        if (Keyboard.current.rightArrowKey.isPressed) moveInput = 1f;

        float currentSpeed = isCrouching ? moveSpeed * crouchSpeedMultiplier : moveSpeed;

        // Move player slightly but clamp within minX/maxX
        float newX = Mathf.Clamp(rb.position.x + moveInput * currentSpeed * Time.deltaTime, minX, maxX);
        rb.position = new Vector2(newX, rb.position.y);

        // Jump
        if (isGrounded && !isCrouching && Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    // Debug: visualize ground check
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position + Vector3.down * 0.6f, new Vector3(0.5f, 0.1f, 0.1f));
    }
}
