using System.Collections.Generic;
using UnityEngine;

public class EnemyCamp : MonoBehaviour
{
    public float campRange = 40f; // Detection range for the entire camp
    private List<Enemy> enemies = new List<Enemy>();
    private Vector3 runeSpawnPosition;
    private GameObject runeFragmentPrefab;
    private bool runeSpawned = false;

    public void Initialize(GameObject runeFragmentPrefab, Vector3 runePosition)
    {
        this.runeFragmentPrefab = runeFragmentPrefab;
        this.runeSpawnPosition = runePosition;
    }

    public void RegisterEnemy(GameObject enemyObject)
    {
        Enemy enemy = enemyObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemies.Add(enemy);
            StartCoroutine(CheckEnemyStatus(enemy));
        }
    }


    private System.Collections.IEnumerator CheckEnemyStatus(Enemy enemy)
    {
        while (enemy != null && enemy.health > 0)
        {
            yield return new WaitForSeconds(0.5f);
        }

        // Enemy has died or been destroyed
        if (enemy == null || enemy.health <= 0)
        {
            enemies.Remove(enemy);
            CheckAllEnemiesDead();
        }
    }

    private void CheckAllEnemiesDead()
    {
        enemies.RemoveAll(enemy => enemy == null || enemy.health <= 0);

        if (enemies.Count == 0 && !runeSpawned)
        {
            SpawnRuneFragment();
        }
    }

    private void SpawnRuneFragment()
    {
        if (runeFragmentPrefab != null)
        {
            Instantiate(runeFragmentPrefab, runeSpawnPosition, Quaternion.identity);
            runeSpawned = true;
            Debug.Log("Rune Fragment spawned - all enemies defeated!");
        }
    }
}