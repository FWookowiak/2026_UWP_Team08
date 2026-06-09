using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildManager : PersistentSingleton<BuildManager>
{
    private TowerConfig towerToBuild;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DeselectTower();
    }

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

        var command = new BuildTowerCommand(node, towerToBuild);
        CommandManager.Instance.Execute(command);
    }

    public void SellTowerOn(Node node)
    {
        if (node.tower == null) return;

        var command = new SellTowerCommand(node);
        CommandManager.Instance.Execute(command);
    }
}