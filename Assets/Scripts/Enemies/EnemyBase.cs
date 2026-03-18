using System;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour{
    public EnemyData enemyData;
    protected float currentHealth;
    protected float currentSpeed;

    protected virtual void Start(){
        currentHealth = enemyData.maxHealth;
        currentSpeed = enemyData.moveSpeed;
    }
    protected virtual void Update(){
        Move();
    }

    protected virtual void Move(){
        
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
    }
    public virtual void TakeDamage(float amount){
        currentHealth -= amount;
        if (currentHealth <= 0){
            Die();
        }
    }

    protected virtual void Die(){
        Debug.Log($"{gameObject.name} zginął! Otrzymujesz {enemyData.goldReward} złota.");
        Destroy(gameObject);
    }

    protected virtual void ReachGoal(){
        Debug.Log($"{gameObject.name} dotarł do bazy! Tracisz {enemyData.damageToPlayer} żyć.");
        Destroy(gameObject);
    }
}
