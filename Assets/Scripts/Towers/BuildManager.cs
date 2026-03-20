using UnityEngine;
using UnityEngine.EventSystems;

public class BuildManager : PersistentSingleton<BuildManager>
{
    private TowerConfig towerToBuild;

    public bool CanBuild { get { return towerToBuild != null; } }
    public bool HasMoney { get { return PlayerStats.Money >= towerToBuild.cost; } }

    public void SelectTowerToBuild(TowerConfig tower)
    {
        towerToBuild = tower;
    }

    public void BuildTowerOn(Node node)
    {
        if (PlayerStats.Money < towerToBuild.cost)
        {
            Debug.Log("Not enough money to build that!");
            return;
        }

        PlayerStats.Money -= towerToBuild.cost;

        GameObject tower = Instantiate(towerToBuild.prefab, node.GetBuildPosition(), Quaternion.identity);
        node.tower = tower;

        Debug.Log("Tower built!");
    }
}
