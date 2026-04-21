using UnityEngine;

public class BuildManager : PersistentSingleton<BuildManager>
{
    private TowerConfig towerToBuild;

    public bool CanBuild => towerToBuild != null;
    public bool HasMoney => towerToBuild != null && PlayerStats.Money >= towerToBuild.cost;
    public TowerConfig SelectedTower => towerToBuild;

    public void SelectTowerToBuild(TowerConfig tower)
    {
        towerToBuild = tower;
    }

    public void DeselectTower()
    {
        towerToBuild = null;
    }

    public void BuildTowerOn(Node node)
    {
        if (towerToBuild == null) return;

        if (PlayerStats.Money < towerToBuild.cost)
        {
            Debug.Log("Za mało złota!");
            return;
        }

        PlayerStats.Money -= towerToBuild.cost;
        GameEvents.MoneyChanged(PlayerStats.Money);

        GameObject tower = Instantiate(
            towerToBuild.prefab, 
            node.GetBuildPosition(), 
            Quaternion.identity
        );
        node.tower = tower;
        node.towerConfig = towerToBuild;

        GameEvents.TowerBuilt(tower, node, towerToBuild.cost);
        Debug.Log($"Zbudowano wieżę! Koszt: {towerToBuild.cost}");
    }

    /// <summary>
    /// Sprzedaż wieży — zwraca część kosztu (50%).
    /// </summary>
    public void SellTowerOn(Node node)
    {
        if (node.tower == null) return;

        int refund = Mathf.RoundToInt(node.towerConfig.cost * 0.5f);
        
        GameEvents.TowerSold(node.tower, node, refund);
        
        PlayerStats.Money += refund;
        GameEvents.MoneyChanged(PlayerStats.Money);

        Destroy(node.tower);
        node.tower = null;
        node.towerConfig = null;

        Debug.Log($"Sprzedano wieżę! Zwrot: {refund}");
    }
}