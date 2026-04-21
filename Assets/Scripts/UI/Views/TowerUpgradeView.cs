using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerUpgradeView : MonoBehaviour, ITowerUpgradeView
{
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private TextMeshProUGUI towerNameText;
    [SerializeField] private TextMeshProUGUI statsText;
    [SerializeField] private Button[] upgradeButtons;
    [SerializeField] private TextMeshProUGUI[] upgradeLabels;
    [SerializeField] private Button[] strategyButtons;

    [Header("Sell")]
    [SerializeField] private Button sellButton;
    [SerializeField] private TextMeshProUGUI sellLabel;

    public event System.Action<int> OnUpgradeClicked;
    public event System.Action<TargetingMode> OnStrategyChanged;
    public event System.Action OnSellClicked;

    private void Start()
    {
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            int index = i;
            upgradeButtons[i].onClick.AddListener(() =>
                OnUpgradeClicked?.Invoke(index));
        }

        for (int i = 0; i < strategyButtons.Length; i++)
        {
            TargetingMode mode = (TargetingMode)i;
            strategyButtons[i].onClick.AddListener(() =>
                OnStrategyChanged?.Invoke(mode));
        }

        if (sellButton != null)
            sellButton.onClick.AddListener(() => OnSellClicked?.Invoke());
    }

    public void ShowPanel(string towerName, float range, float fireRate, float damage)
    {
        upgradePanel.SetActive(true);
        towerNameText.text = towerName;
        statsText.text = $"Zasięg: {range:F1} | Szybkość: {fireRate:F1}/s | Obrażenia: {damage:F0}";
    }

    public void HidePanel()
    {
        upgradePanel.SetActive(false);
    }

    public void UpdateUpgradeButtons(
        TowerUpgradeData[] upgrades, int[] currentLevels, int playerMoney)
    {
        for (int i = 0; i < upgradeButtons.Length && i < upgrades.Length; i++)
        {
            bool canAfford = playerMoney >= upgrades[i].cost;
            bool maxed = currentLevels[i] >= upgrades[i].maxLevel;

            upgradeButtons[i].interactable = canAfford && !maxed;
            upgradeLabels[i].text = maxed
                ? upgrades[i].upgradeName + " (MAX)"
                : upgrades[i].upgradeName + "\n$" + upgrades[i].cost;
        }
    }

    public void SetTargetingMode(TargetingMode mode)
    {
        for (int i = 0; i < strategyButtons.Length; i++)
        {
            var colors = strategyButtons[i].colors;
            colors.normalColor = (int)mode == i
                ? new Color(0.2f, 0.8f, 0.4f, 1f)
                : Color.white;
            strategyButtons[i].colors = colors;
        }
    }

    public void UpdateSellButton(int refundAmount)
    {
        if (sellLabel != null)
            sellLabel.text = $"Sprzedaj\n+${refundAmount}";
    }
}