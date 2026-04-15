using UnityEngine;

public class TowerUpgradePresenter : MonoBehaviour
{
    [SerializeField] private TowerUpgradeView upgradeView;
    [SerializeField] private TowerUpgradeData[] availableUpgrades;

    private TowerBase selectedTower;
    private int[] upgradeLevels;

    private void Start()
    {
        upgradeView.OnUpgradeClicked += HandleUpgrade;
        upgradeView.OnStrategyChanged += HandleStrategyChange;
        upgradeView.HidePanel();
    }

    public void SelectTower(TowerBase tower)
    {
        selectedTower = tower;
        if (tower == null)
        {
            upgradeView.HidePanel();
            return;
        }

        upgradeLevels = new int[availableUpgrades.Length];

        upgradeView.ShowPanel(
            tower.gameObject.name,
            tower.range,
            tower.fireRate,
            tower.GetComponentInChildren<Projectile>() != null ? 10f : 0f
        );

        upgradeView.UpdateUpgradeButtons(
            availableUpgrades, upgradeLevels, PlayerStats.Money
        );

        upgradeView.SetTargetingMode(tower.CurrentTargetingMode);
    }

    private void HandleUpgrade(int index)
    {
        if (selectedTower == null || index >= availableUpgrades.Length) return;

        var data = availableUpgrades[index];
        if (PlayerStats.Money < data.cost) return;
        if (upgradeLevels[index] >= data.maxLevel) return;

        PlayerStats.Money -= data.cost;
        upgradeLevels[index]++;

        selectedTower.range += data.rangeBonus;
        selectedTower.fireRate += data.fireRateBonus;
        // damage w Projectile

        SelectTower(selectedTower); // odśwież panel
    }

    private void HandleStrategyChange(TargetingMode mode)
    {
        if (selectedTower == null) return;
        selectedTower.CurrentTargetingMode = mode;
        upgradeView.SetTargetingMode(mode);
    }

    private void OnDestroy()
    {
        if (upgradeView != null)
        {
            upgradeView.OnUpgradeClicked -= HandleUpgrade;
            upgradeView.OnStrategyChanged -= HandleStrategyChange;
        }
    }
}
