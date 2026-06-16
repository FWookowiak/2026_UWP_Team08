using UnityEngine;

public class TowerUpgradePresenter : MonoBehaviour
{
    [SerializeField] private TowerUpgradeView upgradeView;
    [SerializeField] private TowerUpgradeData[] availableUpgrades;

    private TowerBase selectedTower;
    private Node selectedNode;
    private int[] upgradeLevels;

    private void Awake()
    {
        Debug.Log("[TowerUpgradePresenter] Awake called! I exist in the scene.");
    }

    private void Start()
    {
        Debug.Log($"[TowerUpgradePresenter] Start called. upgradeView is null? {upgradeView == null}");
        if (upgradeView != null)
        {
            upgradeView.OnUpgradeClicked += HandleUpgrade;
            upgradeView.OnStrategyChanged += HandleStrategyChange;
            upgradeView.OnSellClicked += HandleSell;
            upgradeView.HidePanel();
        }
    }
    private void OnEnable()  
    { 
        Debug.Log("[TowerUpgradePresenter] OnEnable called, subscribing to events.");
        GameEvents.OnTowerSelected += OnNodeSelected; 
    }
    private void OnDisable() 
    { 
        Debug.Log("[TowerUpgradePresenter] OnDisable called.");
        GameEvents.OnTowerSelected -= OnNodeSelected; 
    }

    private void OnNodeSelected(TowerBase tower, Node node) => SelectTower(tower, node);

    public void SelectTower(TowerBase tower, Node node = null)
    {
        Debug.Log($"[TowerUpgradePresenter] SelectTower called. Tower: {tower != null}");
        selectedTower = tower;
        selectedNode = node;

        if (tower == null || upgradeView == null)
        {
            if (upgradeView != null) upgradeView.HidePanel();
            return;
        }

        if (availableUpgrades == null)
        {
            availableUpgrades = new TowerUpgradeData[0];
        }

        upgradeLevels = new int[availableUpgrades.Length];
        for (int i = 0; i < availableUpgrades.Length; i++)
        {
            if (availableUpgrades[i] != null && tower.UpgradeLevels != null && tower.UpgradeLevels.TryGetValue(availableUpgrades[i].upgradeName, out int lvl))
                upgradeLevels[i] = lvl;
            else
                upgradeLevels[i] = 0;
        }

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

        var command = new UpgradeTowerCommand(selectedTower, data);
        CommandManager.Instance.Execute(command);

        SelectTower(selectedTower, selectedNode);
    }

    private void HandleStrategyChange(TargetingMode mode)
    {
        if (selectedTower == null) return;

        var command = new ChangeStrategyCommand(selectedTower, mode);
        CommandManager.Instance.Execute(command);
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