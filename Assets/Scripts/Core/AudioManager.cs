using UnityEngine;

public class AudioManager : PersistentSingleton<AudioManager>
{
    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        // Możecie tu np. od razu odpalić muzykę tła po starcie gry
        PlayAmbientSound();
    }
    public void PlayAmbientSound()
    {
        // TODO: Ambience
        Debug.Log("AudioManager: Playing ambient sound...");
    }

    public void StopAmbientSound()
    {
        // TODO: Ambience stop
        Debug.Log("AudioManager: Stopping ambient sound...");
    }

    public void PlaySFX(string clipName)
    {
        // TODO: SFX
        Debug.Log($"AudioManager: Playing SFX - {clipName}");
    }
    
}