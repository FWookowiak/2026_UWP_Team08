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
        
        var upcomingEnemies = WaveManager.Instance.GetUpcomingEnemyTypesForActiveRound();

        if (upcomingEnemies.Count > 0)
        {
            string enemyNames = "";
            for (int i = 0; i < upcomingEnemies.Count; i++)
            {
                enemyNames += upcomingEnemies[i].name;
                if (i < upcomingEnemies.Count - 1) enemyNames += ", ";
            }
    
            upcomingEnemiesText.text = enemyNames; 
        }
    }
}
