using UnityEngine;

public class BoundaryWalls : MonoBehaviour
{
    [Header("References")]
    public Camera mainCamera;       // Assign your Main Camera
    public float wallThickness = 1f;
    public float wallHeight = 20f;
    public float zPos = 0f;

    private GameObject leftWall;
    private GameObject rightWall;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        CreateWalls();
        UpdateWallPositions();
    }

    void LateUpdate()
    {
        // Follow the camera horizontally
        UpdateWallPositions();
    }

    void CreateWalls()
    {
        // --- Left Wall ---
        leftWall = new GameObject("LeftWall");
        BoxCollider2D leftCol = leftWall.AddComponent<BoxCollider2D>();
        leftCol.isTrigger = false;
        leftWall.layer = LayerMask.NameToLayer("Default");

        // --- Right Wall ---
        rightWall = new GameObject("RightWall");
        BoxCollider2D rightCol = rightWall.AddComponent<BoxCollider2D>();
        rightCol.isTrigger = false;
        rightWall.layer = LayerMask.NameToLayer("Default");

        // Set sizes (using scale affects only visuals, not collider size)
        leftCol.size = new Vector2(wallThickness, wallHeight);
        rightCol.size = new Vector2(wallThickness, wallHeight);
    }

    void UpdateWallPositions()
    {
        // Calculate camera boundaries in world space
        float camHeight = 2f * mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        float leftEdge = mainCamera.transform.position.x - camWidth / 2f;
        float rightEdge = mainCamera.transform.position.x + camWidth / 2f;
        float camY = mainCamera.transform.position.y;

        // Position walls at edges
        leftWall.transform.position = new Vector3(leftEdge - wallThickness / 2f, camY, zPos);
        rightWall.transform.position = new Vector3(rightEdge + wallThickness / 2f, camY, zPos);
    }
}
