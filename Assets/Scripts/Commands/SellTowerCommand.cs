using UnityEngine;

public class SellTowerCommand : ICommand
{
    private readonly Node node;
    private readonly TowerConfig config;
    private readonly int refundAmount;
    
    private readonly float savedRange;
    private readonly float savedFireRate;
    private readonly TargetingMode savedMode;

    public string Description => $"Sell tower on {node.name}";

    public SellTowerCommand(Node node)
    {
        this.node = node;
        this.config = node.towerConfig;
        this.refundAmount = Mathf.RoundToInt(config.cost * config.sellRefundPercent);

        var tb = node.tower != null ? node.tower.GetComponent<TowerBase>() : null;
        if (tb != null)
        {
            savedRange = tb.range;
            savedFireRate = tb.fireRate;
            savedMode = tb.CurrentTargetingMode;
        }
    }

    public void Execute()
    {
        if (node.tower == null) return;

        GameEvents.TowerSold(node.tower, node, refundAmount);

        PlayerStats.Money += refundAmount;
        GameEvents.MoneyChanged(PlayerStats.Money);

        Object.Destroy(node.tower);
        node.tower = null;
        node.towerConfig = null;
    }

    public void Undo()
    {
        PlayerStats.Money -= refundAmount;
        GameEvents.MoneyChanged(PlayerStats.Money);

        var rebuilt = Object.Instantiate(config.prefab, node.GetBuildPosition(), Quaternion.identity);
        node.tower = rebuilt;
        node.towerConfig = config;

        var tb = rebuilt.GetComponent<TowerBase>();
        if (tb != null)
        {
            tb.range = savedRange;
            tb.fireRate = savedFireRate;
            tb.CurrentTargetingMode = savedMode;
        }

        GameEvents.TowerBuilt(rebuilt, node, 0);
    }
}