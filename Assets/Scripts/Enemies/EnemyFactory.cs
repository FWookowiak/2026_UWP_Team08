using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    public Transform enemiesContainer;

    public EnemyBase CreateEnemy(GameObject enemyPrefab, Vector3 spawnPosition)
    {
        EnemyBase enemy;

        if (EnemyPool.Instance != null)
        {
            enemy = EnemyPool.Instance.Spawn(enemyPrefab, spawnPosition);
        }
        else
        {
            GameObject go = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, enemiesContainer);
            enemy = go.GetComponent<EnemyBase>();
        }

        enemy.sourcePrefab = enemyPrefab;
        return enemy;
    }
}