using System;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public Animator animator;
    public EnemyData enemyData;
    protected float currentHealth;
    protected float currentSpeed;

    private Transform target;
    private int wavepointIndex = 0;
    private bool isDead = false;

    public Action<float, float> OnHealthChanged;
    
    public float CurrentHealth => currentHealth;
    public int WaypointIndex => wavepointIndex;

    protected virtual void Start()
    {
        currentHealth = enemyData.maxHealth;
        currentSpeed = enemyData.moveSpeed;
        OnHealthChanged?.Invoke(currentHealth, enemyData.maxHealth);

        if (Waypoints.points != null && Waypoints.points.Length > 0)
        {
            target = Waypoints.points[0];
        }
    }

    protected virtual void Update()
    {
        if (isDead) return;

        if (target != null)
        {
            Move();
        
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            bool shouldBeWalking = currentSpeed > 0.1f && distanceToTarget > 0.1f;
        
            if (animator.GetBool("isWalking") != shouldBeWalking)
            {
                animator.SetBool("isWalking", shouldBeWalking);
            }
        }
    }

    protected virtual void Move()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * currentSpeed * Time.deltaTime, Space.World);

        if (dir != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }

        if (Vector3.Distance(transform.position, target.position) <= 0.2f)
            GetNextWaypoint();
    }

    protected void GetNextWaypoint()
    {
        if (wavepointIndex >= Waypoints.points.Length - 1)
        {
            ReachGoal();
            return;
        }
        wavepointIndex++;
        target = Waypoints.points[wavepointIndex];
    }

    public virtual void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        OnHealthChanged?.Invoke(currentHealth, enemyData.maxHealth);

        animator.SetTrigger("GotHit");

        if (currentHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {
        if (isDead) return;
        isDead = true;

        animator.SetTrigger("Die1");

        currentSpeed = 0;
        Collider col = GetComponent<Collider>();
        if (col != null) 
        {
            col.enabled = false;
        }
        else 
        {
            col = GetComponentInChildren<Collider>();
            if (col != null) col.enabled = false;
        }

        PlayerStats.Money += enemyData.goldReward;
        GameEvents.MoneyChanged(PlayerStats.Money);
        GameEvents.EnemyKilled(this, enemyData.goldReward);
        
        WaveManager.Instance.OnEnemyRemoved();

        Destroy(gameObject, 5f); 
    }

    protected virtual void ReachGoal()
    {
        if (isDead) return;
        
        PlayerStats.Lives -= enemyData.damageToPlayer;
        GameEvents.LivesChanged(PlayerStats.Lives);
        GameEvents.EnemyReachedGoal(this, enemyData.damageToPlayer);
        
        if (PlayerStats.Lives <= 0)
            GameManager.Instance.TriggerDefeat();

        WaveManager.Instance.OnEnemyRemoved();
        Destroy(gameObject);
    }
}