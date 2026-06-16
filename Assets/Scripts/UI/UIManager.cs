using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI upcomingEnemiesText;

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

        if (waveText != null){
            waveText.text = WaveManager.Instance.currentRoundIndex.ToString();
        }
        
        var upcomingEnemies = WaveManager.Instance.GetActiveRoundEnemyCounts();

        if (upcomingEnemies != null && upcomingEnemies.Count > 0)
        {
            string enemyNames = "";
            int i = 0;
            foreach (var kvp in upcomingEnemies)
            {
                enemyNames += $"{kvp.Key.name} x{kvp.Value}";
                if (i < upcomingEnemies.Count - 1) 
                {
                    enemyNames += ", ";
                }
                i++;
            }
    
            if (upcomingEnemiesText != null) upcomingEnemiesText.text = enemyNames; 
        }
        else if (upcomingEnemiesText != null)
        {
            upcomingEnemiesText.text = "None";
        }
    }
}
