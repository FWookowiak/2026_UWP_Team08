using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarView : MonoBehaviour, IEnemyHealthBarView
{
    [SerializeField] private Image fillImage;
    [SerializeField] private GameObject healthBarObject;
    [SerializeField] private Gradient colorGradient;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        // Pasek zawsze patrzy na kamerę
        if (mainCamera != null)
        {
            transform.LookAt(
                transform.position + mainCamera.transform.forward
            );
        }
    }

    public void UpdateHealthBar(float normalizedHealth)
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = normalizedHealth;
            fillImage.color = colorGradient.Evaluate(normalizedHealth);
        }
    }

    public void Show()
    {
        if (healthBarObject != null)
            healthBarObject.SetActive(true);
    }

    public void Hide()
    {
        if (healthBarObject != null)
            healthBarObject.SetActive(false);
    }
}
