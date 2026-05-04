using UnityEngine;

public class WaveStarter : MonoBehaviour
{
    private void OnEnable()
    {
        if (InputReader.Instance != null)
            InputReader.Instance.OnStartWavePerformed += StartWave;
    }

    private void OnDisable()
    {
        if (InputReader.Instance != null)
            InputReader.Instance.OnStartWavePerformed -= StartWave;
    }

    private void StartWave()
    {
        if (WaveManager.Instance != null && WaveManager.Instance.enemiesAlive == 0)
            WaveManager.Instance.StartNextRound();
    }
}