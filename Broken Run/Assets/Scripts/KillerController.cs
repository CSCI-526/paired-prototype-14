using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class KillerController : MonoBehaviour
{
    [Header("Ground & Position")]
    public float groundY = -4f;
    public float spawnX = -12.2f;

    [Header("Oscillation Settings")]
    public float oscillationAmplitude = 0.3f;
    public float oscillationSpeed = 0.628f; // radians/sec

    [Header("Explosion Effect")]
    public GameObject explosionPrefab;

    private float startTime;
    private BoxCollider2D solidCollider;
    private BoxCollider2D triggerCollider;

    void Start()
    {
        startTime = Time.time;

        transform.position = new Vector3(spawnX, groundY + 1.5f, 0);
        transform.localScale = new Vector3(1f, 3f, 1f);

        // --- Solid collider for player collisions ---
        solidCollider = gameObject.AddComponent<BoxCollider2D>();
        solidCollider.isTrigger = false;

        // --- Separate trigger collider for obstacle detection ---
        triggerCollider = gameObject.AddComponent<BoxCollider2D>();
        triggerCollider.isTrigger = true;

        // Kinematic Rigidbody2D for manual movement
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        gameObject.tag = "Killer";
    }

    void Update()
    {
        // Oscillate horizontally
        float offset = Mathf.Sin((Time.time - startTime) * oscillationSpeed) * oscillationAmplitude;
        transform.position = new Vector3(spawnX + offset, groundY + 1.5f, 0);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // --- Player collision ---
        if (collision.collider.CompareTag("Player"))
        {
            PlayerController pc = collision.collider.GetComponent<PlayerController>();
            Rigidbody2D prb = collision.collider.GetComponent<Rigidbody2D>();

            if (pc != null && pc.hasShield && prb != null)
            {
                // Strong upward bounce
                Vector2 bounce = new Vector2(0f, pc.shieldBounceForce * 2f);
                prb.linearVelocity = Vector2.zero;
                prb.AddForce(bounce, ForceMode2D.Impulse);

                pc.hasShield = false;
                Debug.Log("Shield absorbed the hit! Player bounced!");
            }
            else
            {
                Debug.Log("Game Over! Player touched the killer!");
                ScoreManager.Instance.GameOver();
                FindObjectOfType<GameOverUI>().ShowGameOver();
                Time.timeScale = 0f;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // --- Obstacle trigger ---
        if (other.CompareTag("Obstacle"))
        {
            if (explosionPrefab != null)
            {
                GameObject fx = Instantiate(explosionPrefab, other.transform.position, Quaternion.identity);
                Destroy(fx, 2f);
            }
            Destroy(other.gameObject);
        }
    }
}
