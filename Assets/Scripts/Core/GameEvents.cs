using System;
using UnityEngine;

/// <summary>
/// Statyczny event bus — wzorzec Observer.
/// Producenci wywołują Invoke, subskrybenci robią += w OnEnable i -= w OnDisable.
/// </summary>
public static class GameEvents
{
    // --- Wieże ---
    public static event Action<GameObject, Node, int> OnTowerBuilt;
    public static event Action<GameObject, Node, int> OnTowerSold;
    public static event Action<TowerBase, Node> OnTowerSelected;
    public static event Action<TowerConfig> OnTowerTypeSelected;
    public static event Action<TowerBase, TargetingMode> OnStrategyChanged;
    public static event Action<TowerBase, TowerUpgradeData> OnTowerUpgraded;
    public static event Action<TowerBase, Vector3> OnTowerShot;

    // --- Wrogowie ---
    public static event Action<EnemyBase, int> OnEnemyKilled;
    public static event Action<EnemyBase, int> OnEnemyReachedGoal;
    public static event Action<EnemyBase, Vector3> OnEnemyHit;
    public static event Action<EnemyState> OnEnemyStateChanged;

    // --- Fale ---
    public static event Action<int, int> OnWaveStarted;
    public static event Action<int> OnWaveCompleted;

    // --- Zasoby ---
    public static event Action<int> OnMoneyChanged;
    public static event Action<int> OnLivesChanged;

    // --- Stan gry ---
    public static event Action<GameState> OnGameStateChanged;

    // ============ Invoke ============

    public static void TowerBuilt(GameObject tower, Node node, int cost)
        => OnTowerBuilt?.Invoke(tower, node, cost);

    public static void TowerSold(GameObject tower, Node node, int refund)
        => OnTowerSold?.Invoke(tower, node, refund);

    public static void TowerSelected(TowerBase tower, Node node)
        => OnTowerSelected?.Invoke(tower, node);

    public static void TowerTypeSelected(TowerConfig config)
        => OnTowerTypeSelected?.Invoke(config);

    public static void StrategyChanged(TowerBase tower, TargetingMode mode)
        => OnStrategyChanged?.Invoke(tower, mode);

    public static void TowerUpgraded(TowerBase tower, TowerUpgradeData data)
        => OnTowerUpgraded?.Invoke(tower, data);

    public static void TowerShot(TowerBase tower, Vector3 position)
        => OnTowerShot?.Invoke(tower, position);

    public static void EnemyKilled(EnemyBase enemy, int goldReward)
        => OnEnemyKilled?.Invoke(enemy, goldReward);

    public static void EnemyReachedGoal(EnemyBase enemy, int damage)
        => OnEnemyReachedGoal?.Invoke(enemy, damage);

    public static void EnemyHit(EnemyBase enemy, Vector3 position)
        => OnEnemyHit?.Invoke(enemy, position);

    public static void EnemyStateChangedEvent(EnemyState state)
        => OnEnemyStateChanged?.Invoke(state);

    public static void WaveStarted(int current, int total)
        => OnWaveStarted?.Invoke(current, total);

    public static void WaveCompleted(int waveIndex)
        => OnWaveCompleted?.Invoke(waveIndex);

    public static void MoneyChanged(int amount)
        => OnMoneyChanged?.Invoke(amount);

    public static void LivesChanged(int amount)
        => OnLivesChanged?.Invoke(amount);

    public static void GameStateChanged(GameState state)
        => OnGameStateChanged?.Invoke(state);
}