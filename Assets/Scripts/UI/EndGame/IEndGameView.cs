using System;

public interface IEndGameView
{
    event Action OnRestartClicked;
    void ShowScreen(string title, string message);
    void HideScreen();
}
