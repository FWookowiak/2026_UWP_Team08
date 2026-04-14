using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialView : MonoBehaviour, ITutorialView
{
    [Header("Popup")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button skipButton;

    [Header("Highlight")]
    [SerializeField] private GameObject highlightOverlay;
    [SerializeField] private RectTransform highlightFrame;

    public event System.Action OnNextClicked;
    public event System.Action OnSkipClicked;

    private void Start()
    {
        nextButton.onClick.AddListener(() => OnNextClicked?.Invoke());
        skipButton.onClick.AddListener(() => OnSkipClicked?.Invoke());
    }

    public void ShowStep(string title, string description)
    {
        popupPanel.SetActive(true);
        titleText.text = title;
        descriptionText.text = description;
    }

    public void HidePopup()
    {
        popupPanel.SetActive(false);
        ClearHighlight();
    }

    public void HighlightElement(GameObject target)
    {
        if (target == null || highlightOverlay == null) return;

        highlightOverlay.SetActive(true);

        // Próbuj dopasować pozycję do obiektu UI
        RectTransform rt = target.GetComponent<RectTransform>();
        if (rt != null && highlightFrame != null)
        {
            highlightFrame.position = rt.position;
            highlightFrame.sizeDelta = rt.sizeDelta + new Vector2(20, 20);
        }
    }

    public void ClearHighlight()
    {
        if (highlightOverlay != null)
            highlightOverlay.SetActive(false);
    }

    public void ShowSkipButton(bool show)
    {
        skipButton.gameObject.SetActive(show);
    }
}
