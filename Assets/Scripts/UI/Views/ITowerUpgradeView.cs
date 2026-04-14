public interface ITowerUpgradeView
{
    void ShowPanel(string towerName, float range, float fireRate, float damage);
    void HidePanel();
    void UpdateUpgradeButtons(TowerUpgradeData[] upgrades, int[] currentLevels, int playerMoney);
    void SetTargetingMode(TargetingMode mode);

    event System.Action<int> OnUpgradeClicked; // index ulepszenia
    event System.Action<TargetingMode> OnStrategyChanged;
}
