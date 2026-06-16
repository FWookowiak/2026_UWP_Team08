using UnityEngine;

public class UpgradeTowerCommand : ICommand
{
    private readonly TowerBase tower;
    private readonly TowerUpgradeData upgradeData;

    public string Description => $"Upgrade {tower.name}: {upgradeData.upgradeName}";

    public UpgradeTowerCommand(TowerBase tower, TowerUpgradeData upgradeData)
    {
        this.tower = tower;
        this.upgradeData = upgradeData;
    }

    public void Execute()
    {
        if (tower == null) return;
        if (PlayerStats.Money < upgradeData.cost) return;

        PlayerStats.Money -= upgradeData.cost;
        GameEvents.MoneyChanged(PlayerStats.Money);

        tower.range += upgradeData.rangeBonus;
        tower.fireRate += upgradeData.fireRateBonus;

        if (tower.UpgradeLevels.ContainsKey(upgradeData.upgradeName))
            tower.UpgradeLevels[upgradeData.upgradeName]++;
        else
            tower.UpgradeLevels[upgradeData.upgradeName] = 1;

        GameEvents.TowerUpgraded(tower, upgradeData);
    }

    public void Undo()
    {
        if (tower == null) return;

        PlayerStats.Money += upgradeData.cost;
        GameEvents.MoneyChanged(PlayerStats.Money);

        tower.range -= upgradeData.rangeBonus;
        tower.fireRate -= upgradeData.fireRateBonus;

        if (tower.UpgradeLevels.ContainsKey(upgradeData.upgradeName))
            tower.UpgradeLevels[upgradeData.upgradeName]--;
    }
}