public interface ITowerUpgradeView
{
    void ShowPanel(string towerName, float range, float fireRate, float damage);
    void HidePanel();
    void UpdateUpgradeButtons(TowerUpgradeData[] upgrades, int[] currentLevels, int playerMoney);
    void SetTargetingMode(TargetingMode mode);
    void UpdateSellButton(int refundAmount);

    event System.Action<int> OnUpgradeClicked;
    event System.Action<TargetingMode> OnStrategyChanged;
    event System.Action OnSellClicked;
}