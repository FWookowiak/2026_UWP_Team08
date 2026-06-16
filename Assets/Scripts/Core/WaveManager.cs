using System.Collections;
using UnityEngine;

public class WaveManager : DestroySingleton<WaveManager>
{
    public EnemyFactory enemyFactory;
    public Transform spawnPoint;
    public WaveData[] allRounds;
    public int currentRoundIndex = 0;
    public int enemiesAlive = 0;

    public int CurrentRoundIndex => currentRoundIndex;
    public int TotalRounds => allRounds != null ? allRounds.Length : 0;

    protected override void Awake()
    {
        base.Awake();
        GenerateProceduralWaves();
    }

    private void GenerateProceduralWaves()
    {
        System.Collections.Generic.List<GameObject> availablePrefabs = new System.Collections.Generic.List<GameObject>();
        if (allRounds != null)
        {
            foreach (var round in allRounds)
            {
                if (round != null && round.waveGroup != null)
                {
                    foreach (var group in round.waveGroup)
                    {
                        if (group != null && group.enemyPrefab != null && !availablePrefabs.Contains(group.enemyPrefab))
                        {
                            availablePrefabs.Add(group.enemyPrefab);
                        }
                    }
                }
            }
        }

        if (availablePrefabs.Count == 0) return;

        int targetRounds = Mathf.Max(6, allRounds != null ? allRounds.Length : 0);
        WaveData[] newRounds = new WaveData[targetRounds];

        for (int i = 0; i < targetRounds; i++)
        {
            if (allRounds != null && i < allRounds.Length && allRounds[i] != null)
            {
                newRounds[i] = allRounds[i];
            }
            else
            {
                WaveData newWave = ScriptableObject.CreateInstance<WaveData>();
                newWave.name = $"ProceduralWave_{i + 1}";
                
                int numGroups = 1 + (i / 2);
                newWave.waveGroup = new WaveGroup[numGroups];
                
                for (int g = 0; g < numGroups; g++)
                {
                    WaveGroup newGroup = ScriptableObject.CreateInstance<WaveGroup>();
                    int prefabIndex = Mathf.Min(i / 2 + g, availablePrefabs.Count - 1);
                    newGroup.enemyPrefab = availablePrefabs[prefabIndex];
                    
                    newGroup.count = 5 + i * 2 + g;
                    newGroup.spawnInterval = Mathf.Max(0.2f, 1.5f - (i * 0.1f));
                    newGroup.delayBeforeNextGroup = 2f;
                    
                    newWave.waveGroup[g] = newGroup;
                }
                newRounds[i] = newWave;
            }
        }
        allRounds = newRounds;
    }

    public void StartNextRound()
    {
        if (currentRoundIndex < allRounds.Length)
        {
            WaveData currentWave = allRounds[currentRoundIndex];
            
            GameEvents.WaveStarted(currentRoundIndex + 1, allRounds.Length);
            
            StartCoroutine(SpawnWaveSequence(currentWave));
            currentRoundIndex++;
        }
        else
        {
            Debug.Log("You won!");
            GameManager.Instance.TriggerVictory();
        }
    }

    private IEnumerator SpawnWaveSequence(WaveData wave)
    {
        foreach (WaveGroup group in wave.waveGroup)
        {
            for (int i = 0; i < group.count; i++)
            {
                EnemyBase spawnedEnemy = enemyFactory.CreateEnemy(
                    group.enemyPrefab, spawnPoint.position
                );
                enemiesAlive++;

                if (i < group.count - 1)
                    yield return new WaitForSeconds(group.spawnInterval);
            }
            yield return new WaitForSeconds(group.delayBeforeNextGroup);
        }
    }

    public void OnEnemyRemoved()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0)
        {
            GameEvents.WaveCompleted(currentRoundIndex - 1);
            
            if (GameManager.Instance.CurrentState == GameState.Defeat) return;

            if (currentRoundIndex < allRounds.Length)
                GameManager.Instance.ChangeState(GameState.BuildPhase);
            else
                GameManager.Instance.TriggerVictory();
        }
    }
    
    public System.Collections.Generic.Dictionary<GameObject, int> GetActiveRoundEnemyCounts()
    {
        var enemyCounts = new System.Collections.Generic.Dictionary<GameObject, int>();
        int activeRoundIdx = currentRoundIndex - 1;

        if (allRounds != null && activeRoundIdx >= 0 && activeRoundIdx < allRounds.Length)
        {
            WaveData currentWave = allRounds[activeRoundIdx];

            if (currentWave != null && currentWave.waveGroup != null)
            {
                foreach (var group in currentWave.waveGroup)
                {
                    if (group != null && group.enemyPrefab != null)
                    {
                        if (enemyCounts.ContainsKey(group.enemyPrefab)){
                            enemyCounts[group.enemyPrefab] += group.count;
                        }else{
                            enemyCounts[group.enemyPrefab] = group.count;
                        }
                    }
                }
            }
        }

        return enemyCounts;
    }
}