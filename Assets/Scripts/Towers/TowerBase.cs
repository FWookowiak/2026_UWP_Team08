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

    public TargetingMode CurrentTargetingMode = TargetingMode.Closest;

    private Transform target;
    private bool isActive = true;

    private void OnEnable()
    {
        GameEvents.OnGameStateChanged += HandleGameStateChanged;
        GameEvents.OnWaveStarted += HandleWaveStarted;
        GameEvents.OnWaveCompleted += HandleWaveCompleted;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStateChanged -= HandleGameStateChanged;
        GameEvents.OnWaveStarted -= HandleWaveStarted;
        GameEvents.OnWaveCompleted -= HandleWaveCompleted;
    }

    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    private void HandleGameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.Victory:
            case GameState.Defeat:
                isActive = false;
                target = null;
                break;
            default:
                isActive = true;
                break;
        }
    }

    private void HandleWaveStarted(int current, int total)
    {
        isActive = true;
    }

    private void HandleWaveCompleted(int waveIndex)
    {
    }

    private void UpdateTarget()
    {
        if (!isActive) return;

        target = TargetingStrategy.GetTarget(
            transform.position, range, enemyTag, CurrentTargetingMode
        );
    }

    private void Update()
    {
        if (!isActive || target == null)
            return;
        
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
        Vector3 rotation = Quaternion.Lerp(
            partToRotate.rotation, lookRotation,
            Time.deltaTime * turnSpeed
        ).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    private void Shoot()
    {
        if (projectilePrefab == null || firePoint == null) return;

        GameObject projGO = Instantiate(
            projectilePrefab, firePoint.position, firePoint.rotation
        );
        Projectile projectile = projGO.GetComponent<Projectile>();
        if (projectile != null)
            projectile.Seek(target);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}