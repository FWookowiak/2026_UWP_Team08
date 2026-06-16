using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : PersistentSingleton<AudioManager>
{
    [Header("Music")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private float musicFadeTime = 1.5f;

    [Header("Dynamic Music Pitch")]
    [SerializeField] private float pitchNormal   = 1.0f;
    [SerializeField] private float pitchTense    = 1.1f;
    [SerializeField] private float pitchCritical = 1.25f;
    [SerializeField] private float pitchLerpSpeed = 2f;
    private float targetPitch = 1f;

    [Header("SFX")]
    [SerializeField] private AudioSource sfxSource;

    [System.Serializable]
    public class SoundEffect
    {
        public string id;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
    }

    // Wpisz tu klipy ręcznie w Inspektorze
    [SerializeField] private List<SoundEffect> soundEffects = new();
    private Dictionary<string, SoundEffect> sfxLookup;

    [Header("Dynamic Music")]
    [SerializeField] private int totalLives = 20;

    protected override void Awake()
    {
        base.Awake();

        sfxLookup = new Dictionary<string, SoundEffect>();
        foreach (var sfx in soundEffects)
        {
            if (!string.IsNullOrEmpty(sfx.id) && sfx.clip != null)
                sfxLookup[sfx.id] = sfx;
        }
    }

    private void OnEnable()
    {
        GameEvents.OnTowerBuilt       += HandleTowerBuilt;
        GameEvents.OnTowerSold        += HandleTowerSold;
        GameEvents.OnTowerUpgraded    += HandleTowerUpgraded;
        GameEvents.OnTowerDestroyed   += HandleTowerDestroyed;
        GameEvents.OnEnemyKilled      += HandleEnemyKilled;
        GameEvents.OnEnemyHit         += HandleEnemyHit;
        GameEvents.OnTowerShot        += HandleTowerShot;
        GameEvents.OnTowerShot        += HandleFrostTowerShot;
        GameEvents.OnWaveStarted      += HandleWaveStarted;
        GameEvents.OnWaveCompleted    += HandleWaveCompleted;
        GameEvents.OnGameStateChanged += HandleGameStateChanged;
        GameEvents.OnLivesChanged     += HandleLivesChanged;
    }

    private void OnDisable()
    {
        GameEvents.OnTowerBuilt       -= HandleTowerBuilt;
        GameEvents.OnTowerSold        -= HandleTowerSold;
        GameEvents.OnTowerUpgraded    -= HandleTowerUpgraded;
        GameEvents.OnTowerDestroyed   -= HandleTowerDestroyed;
        GameEvents.OnEnemyKilled      -= HandleEnemyKilled;
        GameEvents.OnEnemyHit         -= HandleEnemyHit;
        GameEvents.OnTowerShot        -= HandleTowerShot;
        GameEvents.OnTowerShot        -= HandleFrostTowerShot;
        GameEvents.OnWaveStarted      -= HandleWaveStarted;
        GameEvents.OnWaveCompleted    -= HandleWaveCompleted;
        GameEvents.OnGameStateChanged -= HandleGameStateChanged;
        GameEvents.OnLivesChanged     -= HandleLivesChanged;
    }

    private void Start()
    {
        if (musicSource != null && backgroundMusic != null)
        {
            musicSource.clip   = backgroundMusic;
            musicSource.loop   = true;
            musicSource.volume = 1f;
            musicSource.pitch  = pitchNormal;
            musicSource.Play();
        }
    }

    private void Update()
    {
        if (musicSource != null)
            musicSource.pitch = Mathf.Lerp(musicSource.pitch, targetPitch, Time.deltaTime * pitchLerpSpeed);
    }

    // ── Muzyka ───────────────────────────────────────────────────────────────

    public void StopMusic() => musicSource?.Stop();

    // ── SFX ──────────────────────────────────────────────────────────────────

    public void PlaySFX(string id)
    {
        if (sfxLookup == null || sfxSource == null) return;

        if (sfxLookup.TryGetValue(id, out var sfx))
            sfxSource.PlayOneShot(sfx.clip, sfx.volume);
        else
            Debug.LogWarning($"[AudioManager] Brak SFX o id '{id}'");
    }

    // ── Event handlers ────────────────────────────────────────────────────────

    private void HandleTowerBuilt(GameObject t, Node n, int c)       => PlaySFX("tower_build");
    private void HandleTowerSold(GameObject t, Node n, int r)         => PlaySFX("tower_sell");
    private void HandleTowerUpgraded(TowerBase t, TowerUpgradeData d) => PlaySFX("tower_upgrade");
    private void HandleTowerDestroyed(TowerBase t, Vector3 p)         => PlaySFX("tower_destroyed");
    private void HandleEnemyKilled(EnemyBase e, int g)                => PlaySFX("enemy_die");
    private void HandleEnemyHit(EnemyBase e, Vector3 p)               => PlaySFX("enemy_hit");
    private void HandleTowerShot(TowerBase t, Vector3 p)              => PlaySFX("tower_shoot");
    private void HandleFrostTowerShot(TowerBase t, Vector3 p)         => PlaySFX("frost_shoot");
    private void HandleWaveStarted(int c, int t)                      => PlaySFX("wave_start");
    private void HandleWaveCompleted(int w)                           => PlaySFX("wave_complete");

    private void HandleGameStateChanged(GameState state)
    {
        if (state == GameState.Victory) PlaySFX("victory");
        else if (state == GameState.Defeat) PlaySFX("defeat");
    }

    private void HandleLivesChanged(int currentLives)
    {
        float ratio = (float)currentLives / totalLives;

        if (ratio <= 0.15f)      targetPitch = pitchCritical;
        else if (ratio <= 0.5f)  targetPitch = pitchTense;
        else                     targetPitch = pitchNormal;
    }
}