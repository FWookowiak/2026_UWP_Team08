using UnityEngine;

public class AudioManager : PersistentSingleton<AudioManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        // Observer — AudioManager subskrybuje eventy
        GameEvents.OnTowerBuilt += HandleTowerBuilt;
        GameEvents.OnTowerSold += HandleTowerSold;
        GameEvents.OnEnemyKilled += HandleEnemyKilled;
        GameEvents.OnWaveStarted += HandleWaveStarted;
        GameEvents.OnWaveCompleted += HandleWaveCompleted;
        GameEvents.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        GameEvents.OnTowerBuilt -= HandleTowerBuilt;
        GameEvents.OnTowerSold -= HandleTowerSold;
        GameEvents.OnEnemyKilled -= HandleEnemyKilled;
        GameEvents.OnWaveStarted -= HandleWaveStarted;
        GameEvents.OnWaveCompleted -= HandleWaveCompleted;
        GameEvents.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleTowerBuilt(GameObject tower, Node node, int cost)
    {
        PlaySFX("tower_build");
    }

    private void HandleTowerSold(GameObject tower, Node node, int refund)
    {
        PlaySFX("tower_sell");
    }

    private void HandleEnemyKilled(EnemyBase enemy, int gold)
    {
        PlaySFX("enemy_die");
    }

    private void HandleWaveStarted(int current, int total)
    {
        PlaySFX("wave_start");
    }

    private void HandleWaveCompleted(int waveIndex)
    {
        PlaySFX("wave_complete");
    }

    private void HandleGameStateChanged(GameState state)
    {
        if (state == GameState.Victory)
            PlaySFX("victory");
        else if (state == GameState.Defeat)
            PlaySFX("defeat");
    }

    public void PlayAmbientSound()
    {
        Debug.Log("AudioManager: Playing ambient sound...");
    }

    public void StopAmbientSound()
    {
        Debug.Log("AudioManager: Stopping ambient sound...");
    }

    public void PlaySFX(string clipName)
    {
        // TODO: Wczytać AudioClip z Resources lub pola SerializeField
        Debug.Log($"AudioManager: Playing SFX — {clipName}");
    }
}