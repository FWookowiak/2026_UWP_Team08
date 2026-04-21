using UnityEngine;

public class TowerUpgradePresenter : MonoBehaviour
{
    [SerializeField] private TowerUpgradeView upgradeView;
    [SerializeField] private TowerUpgradeData[] availableUpgrades;

    private TowerBase selectedTower;
    private Node selectedNode;
    private int[] upgradeLevels;

    private void Start()
    {
        upgradeView.OnUpgradeClicked += HandleUpgrade;
        upgradeView.OnStrategyChanged += HandleStrategyChange;
        upgradeView.OnSellClicked += HandleSell;
        upgradeView.HidePanel();
    }

    public void SelectTower(TowerBase tower, Node node = null)
    {
        selectedTower = tower;
        selectedNode = node;

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

        // Pokaż cenę sprzedaży
        if (selectedNode != null && selectedNode.towerConfig != null)
        {
            int refund = Mathf.RoundToInt(
                selectedNode.towerConfig.cost * selectedNode.towerConfig.sellRefundPercent
            );
            upgradeView.UpdateSellButton(refund);
        }
    }

    private void HandleUpgrade(int index)
    {
        if (selectedTower == null || index >= availableUpgrades.Length) return;

        var data = availableUpgrades[index];
        if (PlayerStats.Money < data.cost) return;
        if (upgradeLevels[index] >= data.maxLevel) return;

        PlayerStats.Money -= data.cost;
        GameEvents.MoneyChanged(PlayerStats.Money);
        upgradeLevels[index]++;

        selectedTower.range += data.rangeBonus;
        selectedTower.fireRate += data.fireRateBonus;

        SelectTower(selectedTower, selectedNode);
    }

    private void HandleStrategyChange(TargetingMode mode)
    {
        if (selectedTower == null) return;
        selectedTower.CurrentTargetingMode = mode;
        upgradeView.SetTargetingMode(mode);
    }

    private void HandleSell()
    {
        if (selectedNode == null || selectedNode.tower == null) return;

        BuildManager.Instance.SellTowerOn(selectedNode);

        selectedTower = null;
        selectedNode = null;
        upgradeView.HidePanel();
    }

    private void OnDestroy()
    {
        if (upgradeView != null)
        {
            upgradeView.OnUpgradeClicked -= HandleUpgrade;
            upgradeView.OnStrategyChanged -= HandleStrategyChange;
            upgradeView.OnSellClicked -= HandleSell;
        }
    }
}