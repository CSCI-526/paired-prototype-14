using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CollectibleMover : MonoBehaviour
{
    public float speed = 5f;       
    public float despawnX = -20f;  

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; 
    }

    void FixedUpdate()
    {
        
        rb.MovePosition(rb.position + Vector2.left * speed * Time.fixedDeltaTime);

        
        if (rb.position.x < despawnX)
        {
            Destroy(gameObject);
        }
    }
}
