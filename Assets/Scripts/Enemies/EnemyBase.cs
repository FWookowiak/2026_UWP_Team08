using System;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour{
    public EnemyData enemyData;
    protected float currentHealth;
    protected float currentSpeed;
    
    private Transform target;
    private int wavepointIndex = 0;

    protected virtual void Start(){
        currentHealth = enemyData.maxHealth;
        currentSpeed = enemyData.moveSpeed;
        
        if (Waypoints.points != null && Waypoints.points.Length > 0)
        {
            target = Waypoints.points[0];
        }
    }
    protected virtual void Update(){
        if (target != null)
        {
            Move();
        }
    }

    protected virtual void Move(){
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * currentSpeed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.2f)
        {
            GetNextWaypoint();
        }
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
    public virtual void TakeDamage(float amount){
        currentHealth -= amount;
        if (currentHealth <= 0){
            Die();
        }
    }

    protected virtual void Die(){
        Debug.Log($"{gameObject.name} zginął! Otrzymujesz {enemyData.goldReward} złota.");
        PlayerStats.Money += enemyData.goldReward;
        WaveManager.Instance.OnEnemyRemoved();
        Destroy(gameObject);
    }

    protected virtual void ReachGoal(){
        Debug.Log($"{gameObject.name} dotarł do bazy! Tracisz {enemyData.damageToPlayer} żyć.");
        PlayerStats.Lives -= enemyData.damageToPlayer;
        if (PlayerStats.Lives <= 0)
        {
            GameManager.Instance.TriggerDefeat();
        }
        WaveManager.Instance.OnEnemyRemoved();
        Destroy(gameObject);
    }
}
