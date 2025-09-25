using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float minX = -1f;
    public float maxX = 1f;
    public LayerMask groundLayer;

    [Header("Crouch")]
    public float crouchSpeedMultiplier = 0.5f;

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool isCrouching = false;
    private bool controlsFlipped = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 3f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Start()
    {
        StartCoroutine(FlipControlsRoutine());
    }

    void Update()
    {
        // ground check
        isGrounded = Physics2D.OverlapBox(transform.position + Vector3.down * 0.6f,
                                           new Vector2(0.5f, 0.1f), 0f, groundLayer);

        var keyboard = Keyboard.current;
        if (keyboard == null) return; // safety

        // read raw hold + press inputs
        bool rawDownHold = keyboard.downArrowKey.isPressed;
        bool rawUpHold = keyboard.upArrowKey.isPressed;
        bool rawDownPressed = keyboard.downArrowKey.wasPressedThisFrame;
        bool rawUpPressed = keyboard.upArrowKey.wasPressedThisFrame;

        // map crouch & jump according to flipped state:
        // - crouch is a HOLD (isPressed)
        // - jump is a PRESS (wasPressedThisFrame)
        if (controlsFlipped)
        {
            isCrouching = rawUpHold;           // when flipped, Up becomes crouch
        }
        else
        {
            isCrouching = rawDownHold;         // normal: Down is crouch
        }

        bool jumpPressed = controlsFlipped ? rawDownPressed : rawUpPressed; // when flipped, Down becomes jump

        // horizontal input (small/illusionary)
        float moveInput = 0f;
        if (keyboard.leftArrowKey.isPressed) moveInput = -1f;
        if (keyboard.rightArrowKey.isPressed) moveInput = 1f;

        // flip horizontal when needed
        if (controlsFlipped) moveInput *= -1f;

        float currentSpeed = isCrouching ? moveSpeed * crouchSpeedMultiplier : moveSpeed;
        float newX = Mathf.Clamp(rb.position.x + moveInput * currentSpeed * Time.deltaTime, minX, maxX);
        rb.position = new Vector2(newX, rb.position.y);

        // Jump (only if grounded and not crouching)
        if (isGrounded && !isCrouching && jumpPressed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // use rb.velocity
        }
    }

    private IEnumerator FlipControlsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(20f); // normal for 20s
            controlsFlipped = true;
            Debug.Log("⚠️ Controls FLIPPED");
            yield return new WaitForSeconds(10f); // flipped for 10s
            controlsFlipped = false;
            Debug.Log("✅ Controls NORMAL");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position + Vector3.down * 0.6f, new Vector3(0.5f, 0.1f, 0.1f));
    }
}
