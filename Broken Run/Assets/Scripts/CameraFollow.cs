using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 5f;
    public Vector3 offset; // only horizontal offset matters

    private float fixedY;
    private float fixedZ;

    void Start()
    {
        fixedY = transform.position.y; // keep camera Y fixed
        fixedZ = transform.position.z; // keep camera Z fixed
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Slight horizontal follow (small fraction)
        Vector3 targetPos = new Vector3(player.position.x * 0.1f + offset.x, fixedY, fixedZ);
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
    }
}
