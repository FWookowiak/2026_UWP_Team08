using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject{
    public float maxHealth;
    public float moveSpeed;
    public int goldReward;
    public int damageToPlayer;
}