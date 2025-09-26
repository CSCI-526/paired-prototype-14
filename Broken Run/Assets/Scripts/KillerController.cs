using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class KillerController : MonoBehaviour
{
    [Header("Ground & Position")]
    public float groundY = -4f;      // Match EndlessGround
    public float spawnX = -12.2f;      // Fixed X position for killer

    private Rigidbody2D rb;

    void Start()
    {
        // Set killer position
        transform.position = new Vector3(spawnX, groundY + 1.5f, 0);
        transform.localScale = new Vector3(1f, 3f, 1f);

        // Make killer stationary
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static; // Fixed in world

        gameObject.tag = "Killer";
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Game Over! Player touched the killer!");
            Time.timeScale = 0f;
        }
    }
}
