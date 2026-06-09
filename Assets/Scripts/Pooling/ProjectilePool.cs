using UnityEngine;

public class ProjectilePool : PersistentSingleton<ProjectilePool>
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private int initialSize = 30;
    [SerializeField] private Transform poolParent;

    private ObjectPool<Projectile> pool;

    protected override void Awake()
    {
        base.Awake();
        if (poolParent == null) poolParent = transform;
        pool = new ObjectPool<Projectile>(projectilePrefab, initialSize, poolParent);
    }

    public Projectile Spawn(Vector3 position, Quaternion rotation)
    {
        return pool.Get(position, rotation);
    }

    public void Despawn(Projectile projectile)
    {
        pool.Return(projectile);
    }
}