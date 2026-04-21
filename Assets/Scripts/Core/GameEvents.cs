using System;
using UnityEngine;

/// <summary>
/// Statyczny event bus — wzorzec Observer.
/// Producenci wywołują Invoke, subskrybenci robią += w OnEnable i -= w OnDisable.
/// </summary>
public static class GameEvents
{
    // --- Wieże ---
    public static event Action<GameObject, Node, int> OnTowerBuilt;   // tower GO, node, cost
    public static event Action<GameObject, Node, int> OnTowerSold;    // tower GO, node, refund
    
    // --- Wrogowie ---
    public static event Action<EnemyBase, int> OnEnemyKilled;         // enemy, goldReward
    public static event Action<EnemyBase, int> OnEnemyReachedGoal;    // enemy, damage
    
    // --- Fale ---
    public static event Action<int, int> OnWaveStarted;               // currentWave, totalWaves
    public static event Action<int> OnWaveCompleted;                   // waveIndex
    
    // --- Zasoby ---
    public static event Action<int> OnMoneyChanged;                   // newAmount
    public static event Action<int> OnLivesChanged;                   // newAmount
    
    // --- Stan gry ---
    public static event Action<GameState> OnGameStateChanged;         // newState

    // ============ Invoke methods ============

    public static void TowerBuilt(GameObject tower, Node node, int cost)
    {
        OnTowerBuilt?.Invoke(tower, node, cost);
    }

    public static void TowerSold(GameObject tower, Node node, int refund)
    {
        OnTowerSold?.Invoke(tower, node, refund);
    }

    public static void EnemyKilled(EnemyBase enemy, int goldReward)
    {
        OnEnemyKilled?.Invoke(enemy, goldReward);
    }

    public static void EnemyReachedGoal(EnemyBase enemy, int damage)
    {
        OnEnemyReachedGoal?.Invoke(enemy, damage);
    }

    public static void WaveStarted(int current, int total)
    {
        OnWaveStarted?.Invoke(current, total);
    }

    public static void WaveCompleted(int waveIndex)
    {
        OnWaveCompleted?.Invoke(waveIndex);
    }

    public static void MoneyChanged(int newAmount)
    {
        OnMoneyChanged?.Invoke(newAmount);
    }

    public static void LivesChanged(int newAmount)
    {
        OnLivesChanged?.Invoke(newAmount);
    }

    public static void GameStateChanged(GameState newState)
    {
        OnGameStateChanged?.Invoke(newState);
    }
}