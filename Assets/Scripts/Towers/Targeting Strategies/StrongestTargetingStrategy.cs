using System.Linq;
using UnityEngine;

public class StrongestTargetingStrategy : ITargetingStrategy
{
    public string Name => "Najsilniejszy";

    public Transform SelectTarget(Vector3 towerPosition, float range, string enemyTag)
    {
        var enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        return enemies
            .Where(e => Vector3.Distance(towerPosition, e.transform.position) <= range)
            .OrderByDescending(e =>
            {
                var eb = e.GetComponent<EnemyBase>();
                return eb != null ? eb.CurrentHealth : 0;
            })
            .FirstOrDefault()?.transform;
    }
}