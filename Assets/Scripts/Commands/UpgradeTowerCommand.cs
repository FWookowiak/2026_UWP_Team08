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
        // damageBonus aplikowany w Projectile (jeśli używany)
    }

    public void Undo()
    {
        if (tower == null) return;

        PlayerStats.Money += upgradeData.cost;
        GameEvents.MoneyChanged(PlayerStats.Money);

        tower.range -= upgradeData.rangeBonus;
        tower.fireRate -= upgradeData.fireRateBonus;
    }
}