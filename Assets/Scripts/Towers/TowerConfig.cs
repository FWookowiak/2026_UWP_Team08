using UnityEngine;

[System.Serializable]
public class TowerConfig
{
    public GameObject prefab;
    public int cost;
    
    [Range(0f, 1f)]
    public float sellRefundPercent = 0.5f; // 50% zwrotu przy sprzedaży
}