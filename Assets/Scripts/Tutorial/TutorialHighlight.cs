using UnityEngine;
using UnityEngine.UI;

public class TutorialHighlight : MonoBehaviour
{
    [SerializeField] private Image overlayImage;
    [SerializeField] private RectTransform cutoutFrame;

    private float pulseSpeed = 2f;
    private float pulseMin = 0.6f;
    private float pulseMax = 1.0f;

    private void Update()
    {
        if (cutoutFrame == null || !cutoutFrame.gameObject.activeInHierarchy)
            return;

        // Pulsujący efekt ramki
        float alpha = Mathf.Lerp(pulseMin, pulseMax,
            (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);

        Image frameImage = cutoutFrame.GetComponent<Image>();
        if (frameImage != null)
        {
            Color c = frameImage.color;
            c.a = alpha;
            frameImage.color = c;
        }
    }
}
