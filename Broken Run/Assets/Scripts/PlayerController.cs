using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
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

    [Header("Colors")]
    public Color normalColor = Color.white;     // ← set desired normal color in Inspector
    public Color flippedColor = Color.red;      // ← set desired flipped color in Inspector

    private Rigidbody2D rb;
    private SpriteRenderer sr;                  // ← cache SpriteRenderer
    private bool isGrounded = false;
    private bool isCrouching = false;
    private bool controlsFlipped = false;

    [Header("Power-Ups")]
    public bool hasShield = false;
    public float shieldBounceForce = 6f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();    // ← grab the SpriteRenderer
        rb.gravityScale = 3f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        SetPlayerColor(false);                  // start with normal color
    }

    void Start()
    {
        StartCoroutine(FlipControlsRoutine());
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapBox(transform.position + Vector3.down * 0.6f,
                                           new Vector2(0.5f, 0.1f), 0f, groundLayer);

        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        // raw hold & press for Up/Down
        bool rawDownHold = keyboard.downArrowKey.isPressed;
        bool rawUpHold = keyboard.upArrowKey.isPressed;
        bool rawDownPressed = keyboard.downArrowKey.wasPressedThisFrame;
        bool rawUpPressed = keyboard.upArrowKey.wasPressedThisFrame;

        // flip mapping
        if (controlsFlipped)
            isCrouching = rawUpHold;
        else
            isCrouching = rawDownHold;

        bool jumpPressed = controlsFlipped ? rawDownPressed : rawUpPressed;

        // horizontal input
        float moveInput = 0f;
        if (keyboard.leftArrowKey.isPressed) moveInput = -1f;
        if (keyboard.rightArrowKey.isPressed) moveInput = 1f;
        if (controlsFlipped) moveInput *= -1f;

        float currentSpeed = isCrouching ? moveSpeed * crouchSpeedMultiplier : moveSpeed;
        rb.position += new Vector2(moveInput * currentSpeed * Time.deltaTime, 0f);

        if (isGrounded && !isCrouching && jumpPressed)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private IEnumerator FlipControlsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(20f);
            controlsFlipped = true;
            SetPlayerColor(true);                // ← switch to flipped color
            Debug.Log("⚠ Controls FLIPPED");

            yield return new WaitForSeconds(10f);
            controlsFlipped = false;
            SetPlayerColor(false);               // ← back to normal color
            Debug.Log("✅ Controls NORMAL");
        }
    }

    // Change sprite color based on state
    private void SetPlayerColor(bool flipped)
    {
        if (sr != null)
            sr.color = flipped ? flippedColor : normalColor;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position + Vector3.down * 0.6f, new Vector3(0.5f, 0.1f, 0.1f));
    }
}