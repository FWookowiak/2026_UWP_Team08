using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI livesText;

    private void Update()
    {
        if (moneyText != null)
        {
            moneyText.text = "$" + PlayerStats.Money.ToString();
        }
        if (livesText != null)
        {
            livesText.text = PlayerStats.Lives.ToString() + " LIVES";
        }
    }
}
