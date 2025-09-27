using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class KillerController : MonoBehaviour
{
    [Header("Ground & Position")]
    public float groundY = -4f;        // Match EndlessGround
    public float spawnX = -12.2f;      // Fixed center X position for killer

    [Header("Oscillation Settings")]
    public float oscillationAmplitude = 0.3f;      // How far left/right from spawnX
    [Tooltip("Speed in radians/sec. 0.628 ≈ 10 sec per full cycle")]
    public float oscillationSpeed = 0.628f;        // ~10-second oscillation period

    [Header("Explosion Effect")]
    public GameObject explosionPrefab; // Drag your particle prefab here in the Inspector

    private float startTime;

    void Start()
    {
        startTime = Time.time;

        transform.position = new Vector3(spawnX, groundY + 1.5f, 0);
        transform.localScale = new Vector3(1f, 3f, 1f);

        // --- Collider as Trigger ---
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;   // important: detect overlaps instead of physics collisions

        gameObject.tag = "Killer";
    }

    void Update()
    {
        // Slow oscillation (~10 s per full left→right→left cycle)
        float offset = Mathf.Sin((Time.time - startTime) * oscillationSpeed) * oscillationAmplitude;
        transform.position = new Vector3(spawnX + offset, groundY + 1.5f, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // ---- Player touches Killer -> Game Over ----
        if (other.CompareTag("Player"))
        {
            Debug.Log("Game Over! Player touched the killer!");
            Time.timeScale = 0f;
            return;
        }

        // ---- Obstacle touches Killer -> Burst & destroy ----
        if (other.CompareTag("Obstacle"))
        {
            // spawn particle effect at obstacle position
            if (explosionPrefab != null)
            {
                GameObject fx = Instantiate(explosionPrefab, other.transform.position, Quaternion.identity);
                Destroy(fx, 2f); // auto-destroy the particles after 2 seconds
            }

            Destroy(other.gameObject); // remove the obstacle
        }
    }
}
