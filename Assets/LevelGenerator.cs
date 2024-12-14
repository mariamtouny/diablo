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
    public GameObject bossAreaPrefab;
    public GameObject bossPrefab;
    public GameObject treePrefab;
    public GameObject rockPrefab;
    public GameObject house1Prefab;
    public GameObject house2Prefab;
    public Camera mainCamera;

    [Header("Settings")]
    public Vector2 environmentSize = new Vector2(80, 80);

    // Fixed camp positions
    private Vector3[] campPositions = new Vector3[]
    {
        new Vector3(0, -3.5f, 50),      // Camp 1
        new Vector3(150, -5.5f, 100),   // Camp 2
        new Vector3(0, -5, 250),     // Camp 3
        new Vector3(-250, -7, 200)   // Camp 4
    };

    // Fixed tree positions
    private Vector3[] treePositions = new Vector3[]
    {
        new Vector3(-12.7f, 1, 28.4f),
        new Vector3(35.9f, 1, 63.2f),
        new Vector3(-42.1f, 1, 101.6f),
        new Vector3(19.3f, 1, 135.4f),
        new Vector3(-28.5f, 1, 167.2f),
        new Vector3(47.2f, 1, 201.9f),
        new Vector3(-5.8f, 1, 235.1f),
        new Vector3(31.6f, 1, 271.3f),
        new Vector3(-38.4f, 1, 305.7f),
        new Vector3(24.1f, 1, 340.2f),
        new Vector3(-18.9f, 1, 374.5f),
        new Vector3(40.5f, 1, 409.1f),
        new Vector3(-33.7f, 1, 442.8f),
        new Vector3(14.2f, 1, 477.4f),
        new Vector3(-23.3f, 1, 511.6f),
        new Vector3(45.8f, 1, 545.3f),
        new Vector3(-10.4f, 1, 579.1f),
        new Vector3(37.2f, 1, 613.7f),
        new Vector3(-44.6f, 1, 447.4f),
        new Vector3(21.6f, 1, 601.9f)
    };

    // Fixed rock positions
    private Vector3[] rockPositions = new Vector3[]
    {
        new Vector3(-60.2f, 1, 18.7f),
        new Vector3(72.4f, 1, 51.9f),
        new Vector3(-29.1f, 1, 88.3f),
        new Vector3(54.8f, 1, 124.6f),
        new Vector3(-40.3f, 1, 160.4f),
        new Vector3(64.6f, 1, 196.1f),
        new Vector3(-15.2f, 1, 232.5f),
        new Vector3(43.9f, 1, 268.2f),
        new Vector3(-50.5f, 1, 303.9f),
        new Vector3(34.7f, 1, 339.6f),
        new Vector3(-25.4f, 1, 375.4f),
        new Vector3(59.2f, 1, 411.1f),
        new Vector3(-35.8f, 1, 446.8f),
        new Vector3(49.3f, 1, 482.5f),
        new Vector3(-20.9f, 1, 518.3f),
        new Vector3(64.1f, 1, 553.9f),
        new Vector3(-31.6f, 1, 589.6f),
        new Vector3(53.4f, 1, 605.4f),
        new Vector3(-46.2f, 1, 610.9f),
        new Vector3(39.5f, 1, 596.6f)
    };

    // Fixed house positions
    private Vector3 house1Position = new Vector3(-150, 1, 150);
    private Vector3 house2Position = new Vector3(150, 1, 250);

    private Vector3 bossAreaPosition = new Vector3(275, 1, 325);
    private Vector3 bossPosition = new Vector3(275, 1, 320);

    void Start()
    {
        GenerateMainEnvironment();
        GameObject player = GeneratePlayer();
        SetupCamera(player);
        GenerateEnemyCamps();
        GenerateBossArea();
        GenerateTrees();
        GenerateRocks();
        GenerateHouses();
    }

    void GenerateMainEnvironment()
    {
        if (environmentPrefab != null)
        {
            GameObject environment = Instantiate(environmentPrefab, new Vector3(0, 4, 0), Quaternion.identity);
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
        Vector3 playerPosition = new Vector3(0, 0, 0);
        return Instantiate(playerPrefab, playerPosition, Quaternion.identity);
    }

    void SetupCamera(GameObject player)
    {
        if (mainCamera != null)
        {
            // Position the camera behind the player
            Vector3 offset = new Vector3(0, 20, -20);
            mainCamera.transform.position = player.transform.position + offset;

            // Rotate the camera to look at the player and whatever is in front of them
            mainCamera.transform.LookAt(player.transform.position + player.transform.forward * 10);
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
        shadow.transform.localScale = new Vector3(5, 1, 5);

        // Spawn minions, demons, a rune fragment, and potions
        int minionCount = 10;
        int demonCount = Mathf.Max(1, campPositions.Length);

        // Spawn minions within the 50x50 camp area
        for (int i = 0; i < minionCount; i++)
        {
            Vector3 spawnOffset = new Vector3(Random.Range(-15, 20), 5, Random.Range(-10, 20));
            Instantiate(minionPrefab, campPosition + spawnOffset, Quaternion.identity);
        }

        // Spawn demons within the 50x50 camp area
        for (int i = 0; i < demonCount; i++)
        {
            Vector3 spawnOffset = new Vector3(Random.Range(-15, 20), 5, Random.Range(-10, 20));
            Instantiate(demonPrefab, campPosition + spawnOffset, Quaternion.identity);
        }

        // Spawn rune fragment within the 50x50 camp area
        Vector3 spawnOffset2 = new Vector3(Random.Range(-15, 20), 7, Random.Range(-10, 20));
        Instantiate(runeFragmentPrefab, campPosition + spawnOffset2, Quaternion.identity);

        // Spawn potions within the 50x50 camp area
        for (int i = 0; i < 3; i++)
        {
            Vector3 potionPosition = campPosition + new Vector3(Random.Range(-15, 20), 7, Random.Range(-10, 20));
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

    void GenerateTrees()
    {
        // Spawn trees at the fixed positions
        foreach (Vector3 treePosition in treePositions)
        {
            Instantiate(treePrefab, treePosition, Quaternion.identity);
        }
    }

    void GenerateRocks()
    {
        // Spawn rocks at the fixed positions
        foreach (Vector3 rockPosition in rockPositions)
        {
            Instantiate(rockPrefab, rockPosition, Quaternion.identity);
        }
    }

    void GenerateHouses()
    {
        // Spawn the first house at the fixed position
        Instantiate(house1Prefab, house1Position, Quaternion.identity);

        // Spawn the second house at the fixed position
        Instantiate(house2Prefab, house2Position, Quaternion.identity);
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
            1,
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