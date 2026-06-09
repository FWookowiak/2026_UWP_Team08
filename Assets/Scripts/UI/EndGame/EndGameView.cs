using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class EndGameView : MonoBehaviour, IEndGameView
{
    [Header("UI Elements")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button restartButton;

    public event Action OnRestartClicked;

    private void Start()
    {
        if (restartButton != null)
            restartButton.onClick.AddListener(() => OnRestartClicked?.Invoke());
            
        HideScreen();
    }

    public void ShowScreen(string title, string message)
    {
        Debug.Log($"EndGameView.ShowScreen: title={title}, message={message}");
        
        if (popupPanel != null)
        {
            popupPanel.SetActive(true);
        }
        else Debug.LogWarning("EndGameView: Popup Panel is missing!");

        if (titleText != null)
        {
            titleText.text = title;
        }
        else Debug.LogWarning("EndGameView: Title Text is missing!");

        if (messageText != null)
        {
            messageText.text = message;
        }
        else Debug.LogWarning("EndGameView: Message Text is missing!");
    }

    public void HideScreen()
    {
        if (popupPanel != null)
            popupPanel.SetActive(false);
    }
}
