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
        {
            moneyText.text = "$" + amount;
            AnimateTextScale(moneyText);
        }
    }

    public void UpdateLives(int amount)
    {
        if (livesText != null)
        {
            livesText.text = amount + " HP";
            AnimateTextScale(livesText);
        }
    }

    public void UpdateWaveCounter(int current, int total)
    {
        if (waveText != null)
        {
            waveText.text = "Fala " + current + " / " + total;
            AnimateTextScale(waveText);
        }
    }

    public void UpdateGameState(string state)
    {
        if (stateText != null)
        {
            stateText.text = state;
            AnimateTextScale(stateText);
        }
    }

    private void AnimateTextScale(TextMeshProUGUI textElement)
    {
        if (textElement == null || !gameObject.activeInHierarchy) return;
        StartCoroutine(ScaleCoroutine(textElement));
    }

    private System.Collections.IEnumerator ScaleCoroutine(TextMeshProUGUI textElement)
    {
        Vector3 originalScale = Vector3.one;
        Vector3 targetScale = Vector3.one * 1.5f;
        float duration = 0.15f;
        
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            textElement.transform.localScale = Vector3.Lerp(originalScale, targetScale, t / duration);
            yield return null;
        }
        
        t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            textElement.transform.localScale = Vector3.Lerp(targetScale, originalScale, t / duration);
            yield return null;
        }
        
        textElement.transform.localScale = originalScale;
    }
}
