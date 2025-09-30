using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Collectible Settings")]
    public int scoreValue = 50; 
    public GameObject collectEffect; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            if (collectEffect != null)
            {
                Instantiate(collectEffect, transform.position, Quaternion.identity);
            }

            
            ScoreManager.Instance.AddScore(scoreValue);
            Debug.Log("Collected! +" + scoreValue);

            
            Destroy(gameObject);
        }
    }
}
