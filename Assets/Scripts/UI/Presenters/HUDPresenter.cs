using UnityEngine;

public class HUDPresenter : MonoBehaviour
{
    [SerializeField] private HUDView hudView;

    private int lastMoney = -1;
    private int lastLives = -1;

    private void Update()
    {
        if (hudView == null) return;

        // Aktualizuj tylko gdy się zmieni
        if (PlayerStats.Money != lastMoney)
        {
            lastMoney = PlayerStats.Money;
            hudView.UpdateMoney(lastMoney);
        }

        if (PlayerStats.Lives != lastLives)
        {
            lastLives = PlayerStats.Lives;
            hudView.UpdateLives(lastLives);
        }

        // Fale
        if (WaveManager.Instance != null)
        {
            hudView.UpdateWaveCounter(
                WaveManager.Instance.CurrentRoundIndex + 1,
                WaveManager.Instance.TotalRounds
            );
        }

        // Stan gry
        if (GameManager.Instance != null)
        {
            hudView.UpdateGameState(
                GameManager.Instance.CurrentState.ToString()
            );
        }
    }
}
