using UnityEngine;
public class TowerSelector : MonoBehaviour
{
    [Header("Dostępne wieże — konfiguruj tutaj")]
    public TowerConfig[] availableTowers;
    
    public void SelectTower(int index)
    {
        if (BuildManager.Instance == null)
        {
            Debug.LogError("BuildManager.Instance jest null!");
            return;
        }
        if (index < 0 || index >= availableTowers.Length)
        {
            Debug.LogError($"Indeks {index} poza zakresem (masz {availableTowers.Length} wież)");
            return;
        }

        BuildManager.Instance.SelectTowerToBuild(availableTowers[index]);
        GameEvents.TowerTypeSelected(availableTowers[index]); 
        Debug.Log($"Wybrano wieżę: {availableTowers[index].prefab.name}, koszt: {availableTowers[index].cost}");
    }
}