using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    public GameObject explosionPrefab; // assign your particle effect prefab here
    public float bounceForceX = 12f;   // horizontal knockback to the right
    public float bounceForceY = 8f;    // vertical knockback

    void OnCollisionEnter2D(Collision2D collision)
    {
        // ✅ Killer hits obstacle → spawn explosion + destroy
        if (collision.collider.CompareTag("Killer"))
        {
            if (explosionPrefab != null)
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            Destroy(gameObject);
            return;
        }

        // ✅ Player touches Spike → bounce logic
        if (collision.collider.CompareTag("Player") && gameObject.CompareTag("Spike"))
        {
            PlayerController player = collision.collider.GetComponent<PlayerController>();
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

            if (player != null)
            {
                // Always bounce to the right
                rb.linearVelocity = new Vector2(bounceForceX, bounceForceY);

                if (player.hasShield)
                {
                    // Shield absorbs spike, no damage
                    player.hasShield = false;
                    Debug.Log("🛡 Shield absorbed spike! No damage taken.");
                }
                else
                {
                    // No shield → take damage
                    player.TakeDamage(25f);
                }
            }
        }
    }
}
