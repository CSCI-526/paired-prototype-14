using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    private Rigidbody2D rb;

    private bool isGrounded = false;
    private bool isCrouching = false;
    public float crouchSpeedMultiplier = 0.5f; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Crouching
        isCrouching = Keyboard.current.downArrowKey.isPressed;

        // Horizontal movement
        float moveInput = 0f;
        if (Keyboard.current.leftArrowKey.isPressed) moveInput = -1f;
        if (Keyboard.current.rightArrowKey.isPressed) moveInput = 1f;

        float currentSpeed = isCrouching ? moveSpeed * crouchSpeedMultiplier : moveSpeed;
        rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);

        // Jump
        if (isGrounded && !isCrouching && Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }
}
