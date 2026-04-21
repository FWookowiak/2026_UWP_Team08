using System.Collections;
using UnityEngine;

public class WaveManager : DestroySingleton<WaveManager>
{
    public EnemyFactory enemyFactory;
    public Transform spawnPoint;
    public WaveData[] allRounds;
    private int currentRoundIndex = 0;
    public int enemiesAlive = 0;

    public int CurrentRoundIndex => currentRoundIndex;
    public int TotalRounds => allRounds != null ? allRounds.Length : 0;

    protected override void Awake()
    {
        base.Awake();
    }

    public void StartNextRound()
    {
        if (currentRoundIndex < allRounds.Length)
        {
            WaveData currentWave = allRounds[currentRoundIndex];
            
            // Observer — emituj event startu fali
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
            // Observer — emituj event zakończenia fali
            GameEvents.WaveCompleted(currentRoundIndex - 1);
            
            if (currentRoundIndex < allRounds.Length)
                GameManager.Instance.ChangeState(GameState.BuildPhase);
            else
                GameManager.Instance.TriggerVictory();
        }
    }
}