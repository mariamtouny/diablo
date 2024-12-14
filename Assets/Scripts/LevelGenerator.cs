<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
using System.Collections.Generic;
using UnityEngine;
=======
using UnityEngine;
using UnityEngine.AI; // Include AI Navigation namespace
using Unity.AI.Navigation;
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
using UnityEngine;
using UnityEngine.AI; // Include AI Navigation namespace
using Unity.AI.Navigation;
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
using UnityEngine;
using UnityEngine.AI; // Include AI Navigation namespace
using Unity.AI.Navigation;
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
using UnityEngine;
using UnityEngine.AI; // Include AI Navigation namespace
using Unity.AI.Navigation;
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6

public class LevelGenerator : MonoBehaviour
{
    [Header("Prefabs")]
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
    public GameObject environmentPrefab; // Main level environment prefab
    public GameObject playerPrefab;
    public GameObject minionPrefab;
    public GameObject demonPrefab;
    public GameObject potionPrefab;
    public GameObject runeFragmentPrefab;
    //public GameObject gatePrefab;
    public GameObject bossAreaPrefab;

    [Header("Level Settings")]
    public int minCamps = 3;
    public int maxCamps = 5;
    public Vector2 environmentSize = new Vector2(100, 100);
    public Vector2 bossSpawnArea = new Vector2(20, 20);

    [Header("Enemy Settings")]
    public int totalMinions = 40;
    public int totalDemons = 10;
    public int minEnemiesPerCamp = 5;
    public int maxEnemiesPerCamp = 15;

    [Header("Other Settings")]
    public Transform environmentParent;
    public int requiredRuneFragments = 3;

    private List<Vector3> campPositions = new List<Vector3>();

    void Start()
    {
        GenerateEnvironment();
    }

    void GenerateEnvironment()
    {
        GenerateMainEnvironment();
        Vector3 playerPosition = GeneratePlayer();
        SetupCamera(playerPosition);
        GenerateEnemyCamps();
        //GenerateGate();
        GenerateBossArea();
=======
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
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

    //NavMesh Surface
    public NavMeshSurface navMeshSurface;

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

        BakeNavMesh();
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
    }

    void GenerateMainEnvironment()
    {
        if (environmentPrefab != null)
        {
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
            GameObject environment = Instantiate(environmentPrefab, Vector3.zero, Quaternion.identity, environmentParent);
            environment.name = "MainEnvironment";
=======
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
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
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
        }
        else
        {
            Debug.LogError("Environment prefab is not assigned!");
        }
    }

<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
    Vector3 GeneratePlayer()
    {
        Vector3 playerSpawnPosition = new Vector3(environmentSize.x / 2, 0, environmentSize.y / 2);
        Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity, environmentParent);
        return playerSpawnPosition;
    }

    void SetupCamera(Vector3 playerPosition)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            // Set the camera position closer to the player
            Vector3 cameraOffset = new Vector3(-5, 7, -5); // Closer distance, above the player
            mainCamera.transform.position = playerPosition + cameraOffset;

            // Rotate the camera to look at the player
            mainCamera.transform.LookAt(playerPosition);
=======
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
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
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
        }
    }

    void GenerateEnemyCamps()
    {
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
        int campCount = Random.Range(minCamps, maxCamps + 1);
        int remainingMinions = totalMinions;
        int remainingDemons = totalDemons;

        for (int i = 0; i < campCount; i++)
        {
            Vector3 campPosition = GetRandomPosition(environmentSize);
            while (IsPositionTooClose(campPosition, campPositions, 15f))
            {
                campPosition = GetRandomPosition(environmentSize);
            }
            campPositions.Add(campPosition);

            int enemyCount = Random.Range(minEnemiesPerCamp, maxEnemiesPerCamp + 1);
            enemyCount = Mathf.Min(enemyCount, remainingMinions + remainingDemons);

            int demonCount = Mathf.Min(enemyCount / 5, remainingDemons); // ~20% demons
            int minionCount = enemyCount - demonCount;

            remainingMinions -= minionCount;
            remainingDemons -= demonCount;

            GenerateEnemiesInCamp(campPosition, minionCount, demonCount);
            GenerateHealingPotions(campPosition, Random.Range(1, 3));
            GenerateRuneFragment(campPosition);
        }
    }

    void GenerateEnemiesInCamp(Vector3 campPosition, int minionCount, int demonCount)
    {
        for (int i = 0; i < minionCount; i++)
        {
            Vector3 spawnOffset = GetRandomPositionAround(campPosition, 5f);
            Instantiate(minionPrefab, spawnOffset, Quaternion.identity, environmentParent);
        }

        for (int i = 0; i < demonCount; i++)
        {
            Vector3 spawnOffset = GetRandomPositionAround(campPosition, 5f);
            Instantiate(demonPrefab, spawnOffset, Quaternion.identity, environmentParent);
        }
    }

    void GenerateRuneFragment(Vector3 campPosition)
    {
        Vector3 spawnPosition = GetRandomPositionAround(campPosition, 2f);
        GameObject runeFragment = Instantiate(runeFragmentPrefab, spawnPosition, Quaternion.identity, environmentParent);
        runeFragment.SetActive(false); // Rune appears only after all enemies are defeated
    }

    void GenerateHealingPotions(Vector3 campPosition, int potionCount)
    {
        for (int i = 0; i < potionCount; i++)
        {
            Vector3 spawnPosition = GetRandomPositionAround(campPosition, 3f);
            Instantiate(potionPrefab, spawnPosition, Quaternion.identity, environmentParent);
        }
    }

    //void GenerateGate()
    //{
    //    Vector3 gatePosition = new Vector3(environmentSize.x - 10, 0, environmentSize.y - 10);
    //    GameObject gate = Instantiate(gatePrefab, gatePosition, Quaternion.identity, environmentParent);
    //}

    void GenerateBossArea()
    {
        Vector3 bossPosition = GetRandomPosition(bossSpawnArea);
        while (IsPositionTooClose(bossPosition, campPositions, 20f))
        {
            bossPosition = GetRandomPosition(bossSpawnArea);
        }

        Instantiate(bossAreaPrefab, bossPosition, Quaternion.identity, environmentParent);
    }

    Vector3 GetRandomPosition(Vector2 range)
    {
        return new Vector3(Random.Range(0, range.x), 0, Random.Range(0, range.y));
=======
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
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
            Vector3 spawnOffset = new Vector3(Random.Range(-15, 20), 5.5f, Random.Range(-10, 20));
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
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
    }

    Vector3 GetRandomPositionAround(Vector3 center, float radius)
    {
        return center + new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius));
    }

<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
    bool IsPositionTooClose(Vector3 position, List<Vector3> existingPositions, float minDistance)
    {
        foreach (var existing in existingPositions)
        {
            if (Vector3.Distance(position, existing) < minDistance)
            {
                return true;
            }
        }
        return false;
    }
}
=======
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
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

    void BakeNavMesh()
    {
        if (navMeshSurface != null)
        {
            Debug.Log("Baking NavMesh...");
            navMeshSurface.BuildNavMesh(); // Dynamically bake the NavMesh
        }
        else
        {
            Debug.LogError("NavMeshSurface is not assigned or found!");
        }
    }
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
}
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
}
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
}
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
}
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
