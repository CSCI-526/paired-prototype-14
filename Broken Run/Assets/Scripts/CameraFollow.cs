using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Targets")]
    public Transform player;
    public Transform killer;

    [Header("Camera Settings")]
    public float smoothSpeed = 5f;
    public Vector3 offset;

    private float fixedY;
    private float fixedZ;

    void Start()
    {
        fixedY = transform.position.y; // keep Y fixed
        fixedZ = transform.position.z; // keep Z fixed
    }

    void LateUpdate()
    {
        if (player == null || killer == null) return;

        // Find midpoint between player & killer
        Vector3 midpoint = (player.position + killer.position) / 2f;

        // Target position (centered between them, with offset)
        Vector3 targetPos = new Vector3(midpoint.x + offset.x, fixedY, fixedZ);

        // Smooth follow
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
    }
}
