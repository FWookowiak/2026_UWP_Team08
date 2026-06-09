using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target;
    public float speed = 30f;
    public float damage = 10f;

    public void Seek(Transform newTarget)
    {
        target = newTarget;
    }

    private void Update()
    {
        if (target == null)
        {
            ReturnToPool();
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    private void HitTarget()
    {
        ParticleHelper.SpawnExplosion(transform.position, Color.yellow, 5f, 10);
        EnemyBase enemy = target.GetComponent<EnemyBase>();
        if (enemy != null) enemy.TakeDamage(damage);

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        target = null;

        if (ProjectilePool.Instance != null)
            ProjectilePool.Instance.Despawn(this);
        else
            Destroy(gameObject);
    }
}