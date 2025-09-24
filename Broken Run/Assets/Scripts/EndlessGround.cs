using UnityEngine;

public class EndlessGround : MonoBehaviour
{
    [Header("Tiles Settings")]
    public GameObject[] tilePrefabs;   // one or multiple tile prefabs
    public int tilesLeft = 5;          // behind player
    public int tilesRight = 20;        // ahead of player
    public float tileWidth = 10f;      // width of one tile
    public float yPos = -4.5f;         // ground Y position

    [Header("Scrolling")]
    public float scrollSpeed = 5f;

    [Header("Player")]
    public Transform player;           // assign player here

    private Transform[] groundTiles;
    private int totalTiles;
    private float startX = -12.2f;    // X of first tile
    private int recycleIndex = 0;      // track which tile to recycle next

    void Start()
    {
        totalTiles = tilesLeft + tilesRight;
        groundTiles = new Transform[totalTiles];

        // Pre-create all tiles in perfect row
        for (int i = 0; i < totalTiles; i++)
        {
            Vector3 pos = new Vector3(startX + (i - tilesLeft) * tileWidth, yPos, 0);
            GameObject prefab = tilePrefabs[Random.Range(0, tilePrefabs.Length)];
            GameObject tile = Instantiate(prefab, pos, Quaternion.identity);
            groundTiles[i] = tile.transform;
        }

        // Place player on first tile
        player.position = new Vector3(startX + tileWidth / 2f, yPos + 1f, 0);

        // Minimal left boundary for player
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null) pc.minX = startX;
    }

    void Update()
    {
        for (int i = 0; i < totalTiles; i++)
        {
            Transform tile = groundTiles[i];

            // Move tile left
            tile.position += Vector3.left * scrollSpeed * Time.deltaTime;
        }

        // Recycle the tile that went off-screen
        Transform firstTile = groundTiles[recycleIndex];
        if (firstTile.position.x < player.position.x - tileWidth * 2)
        {
            // Determine new X based on last tile's position
            int lastIndex = (recycleIndex + totalTiles - 1) % totalTiles;
            float newX = groundTiles[lastIndex].position.x + tileWidth;

            // Optional tiny Y variation
            float newY = yPos + Random.Range(-0.05f, 0.05f);

            firstTile.position = new Vector3(newX, newY, 0);

            // Move recycleIndex forward
            recycleIndex = (recycleIndex + 1) % totalTiles;
        }
    }
}
