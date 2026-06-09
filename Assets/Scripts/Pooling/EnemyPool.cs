using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyPool : PersistentSingleton<EnemyPool>
{
    [System.Serializable]
    public class EnemyPoolConfig
    {
        public GameObject prefab;
        public int initialSize = 10;
    }

    [SerializeField] private List<EnemyPoolConfig> enemyConfigs;
    [SerializeField] private Transform poolParent;

    private Dictionary<GameObject, ObjectPool<EnemyBase>> poolsByPrefab = new();

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (var pool in poolsByPrefab.Values)
        {
            pool.ReturnAll();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        if (poolParent == null) poolParent = transform;

        foreach (var config in enemyConfigs)
        {
            var enemyComponent = config.prefab.GetComponent<EnemyBase>();
            if (enemyComponent == null)
            {
                Debug.LogError($"[EnemyPool] Prefab {config.prefab.name} nie ma EnemyBase!");
                continue;
            }

            poolsByPrefab[config.prefab] = new ObjectPool<EnemyBase>(
                enemyComponent, config.initialSize, poolParent
            );
        }
    }

    public EnemyBase Spawn(GameObject prefab, Vector3 position)
    {
        if (!poolsByPrefab.ContainsKey(prefab))
        {
            Debug.LogWarning($"[EnemyPool] Brak puli dla {prefab.name}, dodaję w runtime");
            var ec = prefab.GetComponent<EnemyBase>();
            poolsByPrefab[prefab] = new ObjectPool<EnemyBase>(ec, 5, poolParent);
        }

        var enemy = poolsByPrefab[prefab].Get(position, Quaternion.identity);
        enemy.Reset(); 
        return enemy;
    }

    public void Despawn(GameObject prefab, EnemyBase enemy)
    {
        if (poolsByPrefab.ContainsKey(prefab))
            poolsByPrefab[prefab].Return(enemy);
        else
            Object.Destroy(enemy.gameObject);
    }
}