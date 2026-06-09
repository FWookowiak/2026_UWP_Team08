using UnityEngine;
public class SlowingProjectile : Projectile
{
    [Header("Slow Effect")]
    [SerializeField] private float slowAmount = 0.4f;
    [SerializeField] private float slowDuration = 2f;

    public void ConfigureSlow(float amount, float duration)
    {
        slowAmount = amount;
        slowDuration = duration;
    }

    protected override void OnHitEnemy(EnemyBase enemy)
    {
        enemy.ApplySlow(slowAmount, slowDuration);
    }
}