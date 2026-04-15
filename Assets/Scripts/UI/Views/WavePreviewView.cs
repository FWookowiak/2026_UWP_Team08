using UnityEngine;
using TMPro;

public class WavePreviewView : MonoBehaviour, IWavePreviewView
{
    [SerializeField] private GameObject previewPanel;
    [SerializeField] private Transform groupContainer;
    [SerializeField] private GameObject groupItemPrefab;

    public void ShowNextWave(WavePreviewData[] groups)
    {
        if (previewPanel != null)
            previewPanel.SetActive(true);

        // Wyczyść stare wpisy
        foreach (Transform child in groupContainer)
            Destroy(child.gameObject);

        // Stwórz nowe
        foreach (var group in groups)
        {
            GameObject item = Instantiate(groupItemPrefab, groupContainer);
            var text = item.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
                text.text = "T" + group.tier + " x" + group.count;
        }
    }

    public void HidePreview()
    {
        if (previewPanel != null)
            previewPanel.SetActive(false);
    }
}
