using UnityEngine;

public class TowerBase : MonoBehaviour
{
    [Header("Attributes")]
    public float range = 5f;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("Setup")]
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public float turnSpeed = 10f;
    public GameObject projectilePrefab;
    public Transform firePoint;

    [Header("Targeting")]
    [SerializeField] private TargetingMode startMode = TargetingMode.Closest;
    public TargetingMode CurrentTargetingMode { get; set; }

    private ITargetingStrategy strategy;
    protected Transform target;
    private bool isActive = true;
    public System.Collections.Generic.Dictionary<string, int> UpgradeLevels { get; private set; } = new System.Collections.Generic.Dictionary<string, int>();

    private void Start()
    {
        SetTargetingMode(startMode);
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);
    }

    public void SetTargetingMode(TargetingMode mode)
    {
        CurrentTargetingMode = mode;
        strategy = TargetingStrategyFactory.Create(mode);
        GameEvents.StrategyChanged(this, mode);
    }

    private void OnEnable()
    {
        GameEvents.OnGameStateChanged += HandleGameStateChanged;
        GameEvents.OnWaveStarted      += HandleWaveStarted;
        GameEvents.OnWaveCompleted    += HandleWaveCompleted;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStateChanged -= HandleGameStateChanged;
        GameEvents.OnWaveStarted      -= HandleWaveStarted;
        GameEvents.OnWaveCompleted    -= HandleWaveCompleted;
    }

    // wywoływane przez SellTowerCommand / Destroy(gameObject)
    private void OnDestroy()
    {
        GameEvents.TowerDestroyed(this, transform.position);
    }

    private void HandleGameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.Victory:
            case GameState.Defeat:
                isActive = false; target = null;
                break;
            default:
                isActive = true;
                break;
        }
    }

    private void HandleWaveStarted(int c, int t) => isActive = true;
    private void HandleWaveCompleted(int w) { }

    private void UpdateTarget()
    {
        if (!isActive || strategy == null) return;
        target = strategy.SelectTarget(transform.position, range, enemyTag);
    }

    private void Update()
    {
        if (!isActive || target == null) return;

        if (Vector3.Distance(transform.position, target.position) > range)
        {
            target = null;
            return;
        }

        LockOnTarget();

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }
        fireCountdown -= Time.deltaTime;
    }

    private void LockOnTarget()
    {
        if (partToRotate == null) return;
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    protected virtual void Shoot()
    {
        if (projectilePrefab == null || firePoint == null) return;

        GameEvents.TowerShot(this, firePoint.position);

        Projectile projectile;
        if (ProjectilePool.Instance != null){
            projectile = ProjectilePool.Instance.Spawn(firePoint.position, firePoint.rotation);
        }
        else{
            projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation).GetComponent<Projectile>();
        }

        if (projectile != null) projectile.Seek(target);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
    
}