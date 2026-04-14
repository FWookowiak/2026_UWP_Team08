using UnityEngine;

public class EnemyHealthBarPresenter : MonoBehaviour
{
    [SerializeField] private EnemyHealthBarView healthBarView;
    private EnemyBase enemy;

    private void Start()
    {
        enemy = GetComponentInParent<EnemyBase>();
        if (enemy != null)
        {
            enemy.OnHealthChanged += HandleHealthChanged;
        }
    }

    private void HandleHealthChanged(float current, float max)
    {
        if (healthBarView == null) return;

        float normalized = current / max;
        healthBarView.UpdateHealthBar(normalized);

        if (normalized >= 1f)
            healthBarView.Hide();
        else
            healthBarView.Show();
    }

    private void OnDestroy()
    {
        if (enemy != null)
            enemy.OnHealthChanged -= HandleHealthChanged;
    }
}
