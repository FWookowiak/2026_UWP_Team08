using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Centralny manager audio. Subskrybuje GameEvents (Observer) i odtwarza
/// odpowiednie dźwięki / muzykę. Obsługuje też dynamiczny system audio (S-5.10).
/// </summary>
public class AudioManager : PersistentSingleton<AudioManager>
{
    [Header("Music")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip tenseMusic;    // <50% HP
    [SerializeField] private AudioClip criticalMusic; // <15% HP
    [SerializeField] private float musicCrossfadeTime = 1.5f;

    [Header("SFX")]
    [SerializeField] private AudioSource sfxSource;

    [System.Serializable]
    public class SoundEffect
    {
        public string id;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
    }

    [SerializeField] private List<SoundEffect> soundEffects = new();
    private Dictionary<string, SoundEffect> sfxLookup;

    [Header("Dynamic Music (S-5.10)")]
    [SerializeField] private int totalLives = 20;
    private AudioClip currentMusicTrack;

    protected override void Awake()
    {
        base.Awake();

        sfxLookup = new Dictionary<string, SoundEffect>();
        foreach (var sfx in soundEffects)
        {
            if (!string.IsNullOrEmpty(sfx.id) && sfx.clip != null)
                sfxLookup[sfx.id] = sfx;
        }

        EnsureProceduralSFX("tower_shoot", ProceduralAudio.GenerateShootSound());
        EnsureProceduralSFX("enemy_hit", ProceduralAudio.GenerateHitSound());
        EnsureProceduralSFX("enemy_die", ProceduralAudio.GenerateExplosionSound());
        EnsureProceduralSFX("tower_build", ProceduralAudio.GenerateBuildSound());
        EnsureProceduralSFX("tower_sell", ProceduralAudio.GenerateSellSound());
        EnsureProceduralSFX("tower_upgrade", ProceduralAudio.GenerateBuildSound());
        EnsureProceduralSFX("wave_start", ProceduralAudio.GenerateBuildSound());
        EnsureProceduralSFX("wave_complete", ProceduralAudio.GenerateSellSound());
        EnsureProceduralSFX("victory", ProceduralAudio.GenerateBuildSound());
        EnsureProceduralSFX("defeat", ProceduralAudio.GenerateExplosionSound());
    }

    private void EnsureProceduralSFX(string id, AudioClip proceduralClip)
    {
        if (!sfxLookup.ContainsKey(id))
        {
            sfxLookup[id] = new SoundEffect { id = id, clip = proceduralClip, volume = 1f };
        }
    }

    private void OnEnable()
    {
        GameEvents.OnTowerBuilt += HandleTowerBuilt;
        GameEvents.OnTowerSold += HandleTowerSold;
        GameEvents.OnTowerUpgraded += HandleTowerUpgraded;
        GameEvents.OnEnemyKilled += HandleEnemyKilled;
        GameEvents.OnEnemyHit += HandleEnemyHit;
        GameEvents.OnTowerShot += HandleTowerShot;
        GameEvents.OnWaveStarted += HandleWaveStarted;
        GameEvents.OnWaveCompleted += HandleWaveCompleted;
        GameEvents.OnGameStateChanged += HandleGameStateChanged;
        GameEvents.OnLivesChanged += HandleLivesChanged;
    }

    private void OnDisable()
    {
        GameEvents.OnTowerBuilt -= HandleTowerBuilt;
        GameEvents.OnTowerSold -= HandleTowerSold;
        GameEvents.OnTowerUpgraded -= HandleTowerUpgraded;
        GameEvents.OnEnemyKilled -= HandleEnemyKilled;
        GameEvents.OnEnemyHit -= HandleEnemyHit;
        GameEvents.OnTowerShot -= HandleTowerShot;
        GameEvents.OnWaveStarted -= HandleWaveStarted;
        GameEvents.OnWaveCompleted -= HandleWaveCompleted;
        GameEvents.OnGameStateChanged -= HandleGameStateChanged;
        GameEvents.OnLivesChanged -= HandleLivesChanged;
    }

    private void Start()
    {
        PlayMusic(backgroundMusic);
    }

    // ============ MUSIC ============

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource == null) return;
        if (currentMusicTrack == clip) return;

        currentMusicTrack = clip;
        StopAllCoroutines();
        StartCoroutine(CrossfadeMusic(clip));
    }

    private System.Collections.IEnumerator CrossfadeMusic(AudioClip newClip)
    {
        if (musicSource.isPlaying)
        {
            float startVolume = musicSource.volume;
            float t = 0f;
            while (t < musicCrossfadeTime)
            {
                t += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(startVolume, 0f, t / musicCrossfadeTime);
                yield return null;
            }
            musicSource.Stop();
            musicSource.volume = startVolume;
        }

        musicSource.clip = newClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic() => musicSource?.Stop();

    // ============ SFX ============

    public void PlaySFX(string id)
    {
        if (sfxLookup == null || sfxSource == null) return;

        if (sfxLookup.TryGetValue(id, out var sfx))
            sfxSource.PlayOneShot(sfx.clip, sfx.volume);
        else
            Debug.LogWarning($"[AudioManager] Brak SFX o id '{id}'");
    }

    // ============ HANDLERY EVENTÓW ============

    private void HandleTowerBuilt(GameObject t, Node n, int c) => PlaySFX("tower_build");
    private void HandleTowerSold(GameObject t, Node n, int r) => PlaySFX("tower_sell");
    private void HandleTowerUpgraded(TowerBase t, TowerUpgradeData d) => PlaySFX("tower_upgrade");
    private void HandleEnemyKilled(EnemyBase e, int g) => PlaySFX("enemy_die");
    private void HandleEnemyHit(EnemyBase e, Vector3 p) => PlaySFX("enemy_hit");
    private void HandleTowerShot(TowerBase t, Vector3 p) => PlaySFX("tower_shoot");
    private void HandleWaveStarted(int c, int t) => PlaySFX("wave_start");
    private void HandleWaveCompleted(int w) => PlaySFX("wave_complete");

    private void HandleGameStateChanged(GameState state)
    {
        if (state == GameState.Victory) PlaySFX("victory");
        else if (state == GameState.Defeat) PlaySFX("defeat");
    }

    // ============ DYNAMIC AUDIO (S-5.10) ============

    private void HandleLivesChanged(int currentLives)
    {
        float ratio = (float)currentLives / totalLives;

        if (ratio <= 0.15f && criticalMusic != null)
            PlayMusic(criticalMusic);
        else if (ratio <= 0.5f && tenseMusic != null)
            PlayMusic(tenseMusic);
        else if (backgroundMusic != null)
            PlayMusic(backgroundMusic);
    }
}