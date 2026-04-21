using UnityEngine;

/// <summary>
/// HUDPresenter korzysta z wzorca Observer — subskrybuje GameEvents
/// zamiast pollować dane w Update().
/// </summary>
public class HUDPresenter : MonoBehaviour
{
    [SerializeField] private HUDView hudView;

    private void OnEnable()
    {
        GameEvents.OnMoneyChanged += HandleMoneyChanged;
        GameEvents.OnLivesChanged += HandleLivesChanged;
        GameEvents.OnWaveStarted += HandleWaveStarted;
        GameEvents.OnWaveCompleted += HandleWaveCompleted;
        GameEvents.OnGameStateChanged += HandleGameStateChanged;
        GameEvents.OnTowerBuilt += HandleTowerBuilt;
        GameEvents.OnTowerSold += HandleTowerSold;
    }

    private void OnDisable()
    {
        GameEvents.OnMoneyChanged -= HandleMoneyChanged;
        GameEvents.OnLivesChanged -= HandleLivesChanged;
        GameEvents.OnWaveStarted -= HandleWaveStarted;
        GameEvents.OnWaveCompleted -= HandleWaveCompleted;
        GameEvents.OnGameStateChanged -= HandleGameStateChanged;
        GameEvents.OnTowerBuilt -= HandleTowerBuilt;
        GameEvents.OnTowerSold -= HandleTowerSold;
    }

    private void Start()
    {
        // Inicjalizacja stanu
        hudView.UpdateMoney(PlayerStats.Money);
        hudView.UpdateLives(PlayerStats.Lives);
        if (WaveManager.Instance != null)
            hudView.UpdateWaveCounter(1, WaveManager.Instance.TotalRounds);
    }

    private void HandleMoneyChanged(int amount) => hudView.UpdateMoney(amount);
    private void HandleLivesChanged(int amount) => hudView.UpdateLives(amount);
    
    private void HandleWaveStarted(int current, int total) 
        => hudView.UpdateWaveCounter(current, total);

    private void HandleWaveCompleted(int waveIndex)
        => Debug.Log($"HUD: Fala {waveIndex + 1} zakończona");

    private void HandleGameStateChanged(GameState state)
        => hudView.UpdateGameState(state.ToString());

    private void HandleTowerBuilt(GameObject tower, Node node, int cost)
        => Debug.Log($"HUD: Wieża zbudowana za {cost}");

    private void HandleTowerSold(GameObject tower, Node node, int refund)
        => Debug.Log($"HUD: Wieża sprzedana za {refund}");
}