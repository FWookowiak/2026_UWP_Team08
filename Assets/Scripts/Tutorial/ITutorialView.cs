public interface ITutorialView
{
    void ShowStep(string title, string description);
    void HidePopup();
    void HighlightElement(UnityEngine.GameObject target);
    void ClearHighlight();
    void ShowSkipButton(bool show);

    event System.Action OnNextClicked;
    event System.Action OnSkipClicked;
}
