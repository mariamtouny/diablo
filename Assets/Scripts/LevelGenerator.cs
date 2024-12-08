using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject environmentPrefab;
    public GameObject playerPrefab;
    public GameObject minionPrefab;
    public GameObject demonPrefab;
    public GameObject campShadowPrefab;
    public GameObject potionPrefab;
    public GameObject runeFragmentPrefab;
    public GameObject bossAreaPrefab; // Boss area prefab
    public GameObject bossPrefab;     // Boss prefab
    public Camera mainCamera;

    [Header("Settings")]
    public Vector2 environmentSize = new Vector2(80, 80); // Main environment size (80x80)

    // Fixed camp positions
    private Vector3[] campPositions = new Vector3[]
    {
        new Vector3(0, 0, 50),      // Camp 1
        new Vector3(150, 0, 100),   // Camp 2
        new Vector3(0, 0, 250),     // Camp 3
        new Vector3(-250, 0, 200)   // Camp 4 (updated position)
    };

    private Vector3 bossAreaPosition = new Vector3(275, 15, 325);
    private Vector3 bossPosition = new Vector3(265, -5, 315);

    void Start()
    {
        GenerateMainEnvironment();
        GameObject player = GeneratePlayer();
        SetupCamera(player);
        GenerateEnemyCamps();
        GenerateBossArea();
        GenerateRandomPotions(10);
    }

    void GenerateMainEnvironment()
    {
        if (environmentPrefab != null)
        {
            GameObject environment = Instantiate(environmentPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            environment.name = "MainEnvironment";

            // Ensure the environment maintains the correct size
            Renderer environmentRenderer = environment.GetComponent<Renderer>();
            if (environmentRenderer != null)
            {
                Vector3 currentSize = environmentRenderer.bounds.size;
                Vector3 scale = environment.transform.localScale;

                // Adjust scale proportionally to match 80x80 size
                scale.x *= environmentSize.x / currentSize.x;
                scale.z *= environmentSize.y / currentSize.z;
                environment.transform.localScale = scale;
            }
        }
        else
        {
            Debug.LogError("Environment prefab is not assigned!");
        }
    }

    GameObject GeneratePlayer()
    {
        Vector3 playerPosition = new Vector3(0, 0, 0); // Place the player at the center of the environment
        return Instantiate(playerPrefab, playerPosition, Quaternion.identity);
    }

    void SetupCamera(GameObject player)
    {
        if (mainCamera != null)
        {
            // Position the camera behind the player
            Vector3 offset = new Vector3(0, 20, -20); // Camera is 20 units above and 20 units behind
            mainCamera.transform.position = player.transform.position + offset;

            // Rotate the camera to look at the player and whatever is in front of them
            mainCamera.transform.LookAt(player.transform.position + player.transform.forward * 10); // Look slightly ahead of the player
        }
        else
        {
            Debug.LogError("Main Camera is not assigned!");
        }
    }

    void GenerateEnemyCamps()
    {
        foreach (Vector3 campPosition in campPositions)
        {
            GenerateCamp(campPosition);
        }
    }

    void GenerateCamp(Vector3 campPosition)
    {
        // Add a shadow to delineate the camp area
        GameObject shadow = Instantiate(campShadowPrefab, campPosition, Quaternion.identity);
        shadow.transform.localScale = new Vector3(5, 1, 5); // 5x5 shadow for the camp

        // Spawn minions, demons, a rune fragment, and potions
        int minionCount = 10;
        int demonCount = Mathf.Max(1, campPositions.Length); // Number of demons depends on camp index
        float minSpacing = 10f;
        float maxSpacing = 20f;

        // Spawn minions
        for (int i = 0; i < minionCount; i++)
        {
            Vector3 spawnOffset = GetSpacedPosition(campPosition, minSpacing, maxSpacing);
            Instantiate(minionPrefab, spawnOffset, Quaternion.identity);
        }

        // Spawn demons
        for (int i = 0; i < demonCount; i++)
        {
            Vector3 spawnOffset = GetSpacedPosition(campPosition, minSpacing, maxSpacing);
            Instantiate(demonPrefab, spawnOffset, Quaternion.identity);
        }

        // Spawn rune fragment
        Instantiate(runeFragmentPrefab, GetSpacedPosition(campPosition, minSpacing, maxSpacing), Quaternion.identity);

        // Spawn potions
        for (int i = 0; i < 3; i++)
        {
            Vector3 potionPosition = GetSpacedPosition(campPosition, minSpacing, maxSpacing);
            Instantiate(potionPrefab, potionPosition, Quaternion.identity);
        }
    }

    void GenerateBossArea()
    {
        if (bossAreaPrefab != null)
        {
            // Instantiate the boss area
            GameObject bossArea = Instantiate(bossAreaPrefab, bossAreaPosition, Quaternion.identity);
            bossArea.name = "BossArea";

            // Instantiate the boss (independent of the boss area)
            if (bossPrefab != null)
            {
                Instantiate(bossPrefab, bossPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogError("Boss prefab is not assigned!");
            }

            // Spawn potions in the boss area
            for (int i = 0; i < 5; i++)
            {
                Vector3 potionPosition = GetRandomPositionAround(bossAreaPosition, 10f);
                Instantiate(potionPrefab, potionPosition, Quaternion.identity);
            }
        }
        else
        {
            Debug.LogError("Boss area prefab is not assigned!");
        }
    }

    void GenerateRandomPotions(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 randomPosition = GetRandomPositionWithinEnvironment();
            Instantiate(potionPrefab, randomPosition, Quaternion.identity);
        }
    }

    Vector3 GetRandomPositionAround(Vector3 center, float radius)
    {
        return center + new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius));
    }

    Vector3 GetSpacedPosition(Vector3 center, float minSpacing, float maxSpacing)
    {
        float spacing = Random.Range(minSpacing, maxSpacing);
        return center + new Vector3(
            Random.Range(-spacing, spacing),
            0,
            Random.Range(-spacing, spacing)
        );
    }

    Vector3 GetRandomPositionWithinEnvironment()
    {
        float halfWidth = environmentSize.x / 2;
        float halfHeight = environmentSize.y / 2;

        return new Vector3(
            Random.Range(-halfWidth, halfWidth),
            0,
            Random.Range(-halfHeight, halfHeight)
        );
    }
}
