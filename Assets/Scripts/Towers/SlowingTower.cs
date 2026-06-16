using UnityEngine;

public class SlowingTower : TowerBase
{
    [Header("Slowing")]
    [SerializeField] private float slowAmount = 0.4f;
    [SerializeField] private float slowDuration = 2f;

    public float SlowAmount => slowAmount;
    public float SlowDuration => slowDuration;
    
    protected override void Shoot()
    {
        if (projectilePrefab == null || firePoint == null) return;

        GameEvents.FrostTowerShot(this, firePoint.position);
        ParticleHelper.SpawnFreezeExplosion(firePoint.position, Color.cyan, 2f, 8);

        Projectile projectile;
        if (ProjectilePool.Instance != null){
            projectile = ProjectilePool.Instance.Spawn(firePoint.position, firePoint.rotation);
        }
        else{
            projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation).GetComponent<Projectile>();
        }

        if (projectile != null) 
        {
            projectile.Seek(target);
            if (projectile is SlowingProjectile slowingProj)
            {
                slowingProj.ConfigureSlow(slowAmount, slowDuration);
            }
        }
    }
}