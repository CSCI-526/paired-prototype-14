using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class LeftBoundary : MonoBehaviour
{
    private GameOverManager gameOverManager;

    void Start()
    {
        // 
        var collider = GetComponent<BoxCollider2D>();
        collider.isTrigger = true;

        // GameOverManager
        gameOverManager = FindObjectOfType<GameOverManager>();

        gameObject.tag = "LeftBoundary";
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Game Over! Player hit the left boundary!");
            gameOverManager.TriggerGameOver();
        }
    }
}
