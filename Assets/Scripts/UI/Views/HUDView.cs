using UnityEngine;
using TMPro;

public class HUDView : MonoBehaviour, IHUDView
{
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI stateText;

    public void UpdateMoney(int amount)
    {
        if (moneyText != null)
            moneyText.text = "$" + amount;
    }

    public void UpdateLives(int amount)
    {
        if (livesText != null)
            livesText.text = amount + " HP";
    }

    public void UpdateWaveCounter(int current, int total)
    {
        if (waveText != null)
            waveText.text = "Fala " + current + " / " + total;
    }

    public void UpdateGameState(string state)
    {
        if (stateText != null)
            stateText.text = state;
    }
}
