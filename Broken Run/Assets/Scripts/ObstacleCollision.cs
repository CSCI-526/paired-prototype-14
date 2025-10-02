using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    public GameObject explosionPrefab; // assign your particle effect prefab here

    void OnCollisionEnter2D(Collision2D collision)
    {
        // ✅ Killer hits obstacle → spawn explosion + destroy
        if (collision.collider.CompareTag("Killer"))
        {
            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
            return;
        }

        // ✅ Player touches Spike → take 25 damage
        if (collision.collider.CompareTag("Player") && gameObject.CompareTag("Spike"))
        {
            PlayerController player = collision.collider.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(25f);
            }
        }
    }
}
