using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public Transform player;
    public EndlessGround groundManager;
    public float spawnDistance = 20f;
    public float spawnInterval = 2f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnObstacle();
        }
    }

    void SpawnObstacle()
    {
        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        if (prefab == null) return;

        float spawnX = player.position.x + spawnDistance;

        // Find a tile under spawnX to place the obstacle
        Transform tileToSpawnOn = null;
        foreach (var tile in groundManager.groundTiles)
        {
            if (tile == null) continue;
            float left = tile.position.x - groundManager.tileWidth / 2f;
            float right = tile.position.x + groundManager.tileWidth / 2f;
            if (spawnX >= left && spawnX <= right)
            {
                tileToSpawnOn = tile;
                break;
            }
        }

        if (tileToSpawnOn == null) return;

        Vector3 spawnPos = new Vector3(spawnX, tileToSpawnOn.position.y + 0.5f, 0);
        GameObject obstacle = Instantiate(prefab, spawnPos, Quaternion.identity);

        Rigidbody2D rb = obstacle.GetComponent<Rigidbody2D>();
        if (rb == null) rb = obstacle.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        ObstacleMover mover = obstacle.AddComponent<ObstacleMover>();
        mover.speed = groundManager.scrollSpeed;
        mover.despawnX = player.position.x - 20f;
    }
}
