using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Prefabs")]
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
    }

    void GenerateMainEnvironment()
    {
        if (environmentPrefab != null)
        {
            GameObject environment = Instantiate(environmentPrefab, Vector3.zero, Quaternion.identity, environmentParent);
            environment.name = "MainEnvironment";
        }
        else
        {
            Debug.LogError("Environment prefab is not assigned!");
        }
    }

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
        }
    }

    void GenerateEnemyCamps()
    {
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
    }

    Vector3 GetRandomPositionAround(Vector3 center, float radius)
    {
        return center + new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius));
    }

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
