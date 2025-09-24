using UnityEngine;

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

    // Update is called once per frame
    void Update()
    {
        //Crouching
        isCrouching = Input.GetKey(KeyCode.DownArrow);
        //Left and right
        float moveInput = Input.GetAxis("Horizontal");
        float currentSpeed = isCrouching ? moveSpeed * crouchSpeedMultiplier : moveSpeed;
        rb.velocity = new Vector2(moveInput * currentSpeed, rb.velocity.y);

        //jump
        if (isGrounded && !isCrouching && Input.GetKeyDown(KeyCode.UpArrow))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }


    }
        void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}


