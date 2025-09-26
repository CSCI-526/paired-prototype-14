using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ObstacleMover : MonoBehaviour
{
    public float speed = 5f;        // match ground scroll speed
    public float despawnX = -20f;   // when to destroy obstacle

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; // Prevents pushing player
    }

    void FixedUpdate()
    {
        // Move left using physics-safe method
        rb.MovePosition(rb.position + Vector2.left * speed * Time.fixedDeltaTime);

        // Destroy if out of screen
        if (rb.position.x < despawnX)
        {
            Destroy(gameObject);
        }
    }
}
