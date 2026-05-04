using UnityEngine;

public class BuildTowerCommand : ICommand
{
    private readonly Node node;
    private readonly TowerConfig config;
    private GameObject builtTower;

    public string Description => $"Build {config.prefab.name} on {node.name}";

    public BuildTowerCommand(Node node, TowerConfig config)
    {
        this.node = node;
        this.config = config;
    }

    public void Execute()
    {
        if (PlayerStats.Money < config.cost)
        {
            Debug.Log("Za mało złota!");
            return;
        }

        PlayerStats.Money -= config.cost;
        GameEvents.MoneyChanged(PlayerStats.Money);

        builtTower = Object.Instantiate(config.prefab, node.GetBuildPosition(), Quaternion.identity);
        node.tower = builtTower;
        node.towerConfig = config;

        GameEvents.TowerBuilt(builtTower, node, config.cost);
    }

    public void Undo()
    {
        if (builtTower == null) return;

        // Pełny zwrot przy undo (nie 50% jak przy normalnej sprzedaży)
        PlayerStats.Money += config.cost;
        GameEvents.MoneyChanged(PlayerStats.Money);

        GameEvents.TowerSold(builtTower, node, config.cost);

        Object.Destroy(builtTower);
        node.tower = null;
        node.towerConfig = null;
        builtTower = null;
    }
}