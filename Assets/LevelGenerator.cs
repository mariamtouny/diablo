using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject environmentPrefab; // Full environment prefab
    public GameObject playerPrefab;
    public GameObject minionPrefab;
    public GameObject demonPrefab;
    public GameObject potionPrefab;
    public GameObject runeFragmentPrefab;
    public GameObject bossAreaPrefab;
    public GameObject campShadowPrefab; // Shadow to highlight camp areas

    [Header("Level Settings")]
    public Vector2 environmentSize = new Vector2(100, 100);
    public Vector2 bossSpawnArea = new Vector2(20, 20);

    [Header("Enemy Camps")]
    public Vector3 playerStartPosition;
    public int numberOfCamps = 4;
    public float campSpacing = 15f; // Space between camps
    public float minDistanceFromPlayer = 8f;

    [Header("Boss Area")]
    public int bossPotions = 5;

    [Header("Other Settings")]
    public Transform environmentParent;

    private List<Vector3> campPositions = new List<Vector3>();

    void Start()
    {
        GenerateEnvironment();
    }

    void GenerateEnvironment()
    {
        GenerateMainEnvironment();
        playerStartPosition = GeneratePlayer();
        GenerateEnemyCamps();
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

    void GenerateEnemyCamps()
    {
        for (int i = 0; i < numberOfCamps; i++)
        {
            Vector3 campPosition;
            do
            {
                campPosition = GetRandomPosition(environmentSize);
            } while (IsPositionTooClose(campPosition, campPositions, campSpacing) ||
                     Vector3.Distance(campPosition, playerStartPosition) < minDistanceFromPlayer);

            campPositions.Add(campPosition);
            GenerateCamp(campPosition, i + 1);
        }
    }

    void GenerateCamp(Vector3 campPosition, int campIndex)
    {
        // Create a shadow to delineate the camp area
        GameObject shadow = Instantiate(campShadowPrefab, campPosition, Quaternion.identity, environmentParent);
        shadow.transform.localScale = new Vector3(5, 1, 5); // Scale to fit the 5x5 area

        int minionCount = 10;
        int demonCount = campIndex; // Increase demons per camp

        for (int i = 0; i < minionCount; i++)
        {
            Vector3 spawnOffset = GetRandomPositionAround(campPosition, 2.5f); // Spread out enemies within the camp
            Instantiate(minionPrefab, spawnOffset, Quaternion.identity, environmentParent);
        }

        for (int i = 0; i < demonCount; i++)
        {
            Vector3 spawnOffset = GetRandomPositionAround(campPosition, 2.5f);
            Instantiate(demonPrefab, spawnOffset, Quaternion.identity, environmentParent);
        }

        GenerateRuneFragment(campPosition);
        GenerateHealingPotions(campPosition, Random.Range(1, 3)); // Generate potions in the camp
    }

    void GenerateRuneFragment(Vector3 campPosition)
    {
        Vector3 spawnPosition = GetRandomPositionAround(campPosition, 1f);
        GameObject runeFragment = Instantiate(runeFragmentPrefab, spawnPosition, Quaternion.identity, environmentParent);
        runeFragment.SetActive(false); // Rune appears only after all enemies are defeated
    }

    void GenerateHealingPotions(Vector3 areaCenter, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPosition = GetRandomPositionAround(areaCenter, 4f);
            Instantiate(potionPrefab, spawnPosition, Quaternion.identity, environmentParent);
        }
    }

    void GenerateBossArea()
    {
        Vector3 bossPosition;
        do
        {
            bossPosition = GetRandomPosition(bossSpawnArea);
        } while (Vector3.Distance(bossPosition, playerStartPosition) < minDistanceFromPlayer);

        GameObject bossArea = Instantiate(bossAreaPrefab, bossPosition, Quaternion.identity, environmentParent);

        // Scatter potions within the boss area
        for (int i = 0; i < bossPotions; i++)
        {
            Vector3 spawnOffset = GetRandomPositionAround(bossPosition, 5f);
            Instantiate(potionPrefab, spawnOffset, Quaternion.identity, environmentParent);
        }
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
