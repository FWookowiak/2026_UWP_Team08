using System.Linq;
using UnityEngine;

public class WeakestTargetingStrategy : ITargetingStrategy
{
    public string Name => "Najsłabszy";

    public Transform SelectTarget(Vector3 towerPosition, float range, string enemyTag)
    {
        var enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        return enemies
            .Where(e => Vector3.Distance(towerPosition, e.transform.position) <= range)
            .OrderBy(e =>
            {
                var eb = e.GetComponent<EnemyBase>();
                return eb != null ? eb.CurrentHealth : float.MaxValue;
            })
            .FirstOrDefault()?.transform;
    }
}