using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class KillerController : MonoBehaviour
{
    [Header("Ground & Position")]
    public float groundY = -4f;
    public float spawnX = -12.2f;

    [Header("Oscillation Settings")]
    public float oscillationAmplitude = 0.3f;
    public float oscillationSpeed = 0.628f;

    [Header("Explosion Effect")]
    public GameObject explosionPrefab;

    private float startTime;

    void Start()
    {
        startTime = Time.time;

        transform.position = new Vector3(spawnX, groundY + 1.5f, 0);
        transform.localScale = new Vector3(1f, 3f, 1f);

        // Collider is NOT a trigger now → solid wall
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        col.isTrigger = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        gameObject.tag = "Killer";
    }

    void Update()
    {
        float offset = Mathf.Sin((Time.time - startTime) * oscillationSpeed) * oscillationAmplitude;
        transform.position = new Vector3(spawnX + offset, groundY + 1.5f, 0);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerController pc = collision.collider.GetComponent<PlayerController>();
            if (pc != null && pc.hasShield)
            {
                // Bounce instead of passing through
                Rigidbody2D prb = pc.GetComponent<Rigidbody2D>();
                if (prb != null)
                {
                    prb.linearVelocity = new Vector2(prb.linearVelocity.x, pc.shieldBounceForce);
                }

                pc.hasShield = false; // consume shield
                Debug.Log("Shield absorbed the hit! Player bounced.");
            }
            else
            {
                Debug.Log("Game Over! Player touched the killer!");
                ScoreManager.Instance.GameOver();
                FindObjectOfType<GameOverUI>().ShowGameOver();
                Time.timeScale = 0f;
            }
        }

        if (collision.collider.CompareTag("Obstacle"))
        {
            if (explosionPrefab != null)
            {
                GameObject fx = Instantiate(explosionPrefab, collision.transform.position, Quaternion.identity);
                Destroy(fx, 2f);
            }
            Destroy(collision.gameObject);
        }
    }
}
