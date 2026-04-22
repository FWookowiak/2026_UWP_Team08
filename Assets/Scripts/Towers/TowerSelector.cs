using UnityEngine;
public class TowerSelector : MonoBehaviour
{
    [Header("Dostępne wieże — konfiguruj tutaj")]
    public TowerConfig[] availableTowers;
 
    /// <summary>
    /// Wywoływane przez Button.OnClick() z indeksem wieży.
    /// </summary>
    public void SelectTower(int index)
    {
        if (BuildManager.Instance == null)
        {
            Debug.LogError("TowerSelector: BuildManager.Instance jest null! Czy jest w scenie?");
            return;
        }
 
        if (index < 0 || index >= availableTowers.Length)
        {
            Debug.LogError($"TowerSelector: Indeks {index} poza zakresem! Masz {availableTowers.Length} wież.");
            return;
        }
 
        BuildManager.Instance.SelectTowerToBuild(availableTowers[index]);
        Debug.Log($"Wybrano wieżę: {availableTowers[index].prefab.name}, koszt: {availableTowers[index].cost}");
    }
}