using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    public GameObject explosionPrefab; // assign your particle effect prefab here

    void OnCollisionEnter2D(Collision2D collision)
    {
        // ✅ Only react if the object that hit this obstacle is the Killer
        if (collision.collider.CompareTag("Killer"))
        {
            // spawn particle burst at this obstacle's position
            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }

            // destroy the obstacle
            Destroy(gameObject);
        }

        // ❌ If Player touches, do nothing
    }
}
