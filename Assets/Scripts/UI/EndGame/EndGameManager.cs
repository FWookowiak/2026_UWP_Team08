using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour
{
    [SerializeField] private EndGameView endGameView;

    private void OnEnable()
    {
        GameEvents.OnGameStateChanged += HandleGameStateChanged;
        if (endGameView != null)
        {
            endGameView.OnRestartClicked += RestartGame;
        }
    }

    private void OnDisable()
    {
        GameEvents.OnGameStateChanged -= HandleGameStateChanged;
        if (endGameView != null)
        {
            endGameView.OnRestartClicked -= RestartGame;
        }
    }

    private void HandleGameStateChanged(GameState state)
    {
        Debug.Log($"EndGameManager received state: {state}");
        
        if (endGameView == null) 
        {
            Debug.LogError("EndGameManager: EndGameView is NOT assigned in the inspector!");
            return;
        }

        if (state == GameState.Victory)
        {
            endGameView.ShowScreen("ZWYCIĘSTWO!", "Udało ci się obronić bazę przed wszystkimi falami wrogów.");
        }
        else if (state == GameState.Defeat)
        {
            endGameView.ShowScreen("PORAŻKA!", "Twoja baza została zniszczona.");
        }
    }

    private void RestartGame()
    {
        // Resume time in case it was frozen by GameManager
        Time.timeScale = 1f;
        // Reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
