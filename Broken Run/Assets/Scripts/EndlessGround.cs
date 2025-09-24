using UnityEngine;

public class EndlessGround : MonoBehaviour
{
    [Header("Tiles Settings")]
    public GameObject[] tilePrefabs;
    public int tilesLeft = 5;          
    public int tilesRight = 20;        
    public float tileWidth = 10f;      
    public float yPos = -4.5f;         

    [Header("Scrolling")]
    public float scrollSpeed = 5f;

    [Header("Player")]
    public Transform player;           

    private Transform[] groundTiles;
    private float[] tileYPositions;     // store Y variation for each tile
    private int totalTiles;
    private float startX = -12.2f;
    private float leftBoundary = -30f; // when tile is far left

    void Start()
    {
        totalTiles = tilesLeft + tilesRight;
        groundTiles = new Transform[totalTiles];
        tileYPositions = new float[totalTiles];

        // Pre-create all tiles
        for (int i = 0; i < totalTiles; i++)
        {
            Vector3 pos = new Vector3(startX + (i - tilesLeft) * tileWidth, yPos, 0);
            GameObject prefab = tilePrefabs[Random.Range(0, tilePrefabs.Length)];

            // store Y variation once
            float yVar = Random.Range(-0.05f, 0.05f);
            tileYPositions[i] = yPos + yVar;

            GameObject tile = Instantiate(prefab, new Vector3(pos.x, tileYPositions[i], 0), Quaternion.identity);
            groundTiles[i] = tile.transform;
        }

        // Place player on first right tile
        player.position = new Vector3(startX + tileWidth / 2f, yPos + 1f, 0);

        // Player minimal X
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null) pc.minX = startX;
    }

    void Update()
    {
        for (int i = 0; i < totalTiles; i++)
        {
            Transform tile = groundTiles[i];

            // move left
            tile.position += Vector3.left * scrollSpeed * Time.deltaTime;

            // recycle when off-screen
            if (tile.position.x < leftBoundary)
            {
                // find rightmost tile
                float maxX = float.MinValue;
                for (int j = 0; j < totalTiles; j++)
                {
                    if (groundTiles[j].position.x > maxX) maxX = groundTiles[j].position.x;
                }

                // move current tile to the right of the farthest tile
                tile.position = new Vector3(maxX + tileWidth, tileYPositions[i], 0);
            }
        }
    }
}
