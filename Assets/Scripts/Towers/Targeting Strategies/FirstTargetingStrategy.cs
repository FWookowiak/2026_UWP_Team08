using System.Linq;
using UnityEngine;

public class FirstTargetingStrategy : ITargetingStrategy
{
    public string Name => "Pierwszy na ścieżce";

    public Transform SelectTarget(Vector3 towerPosition, float range, string enemyTag)
    {
        var enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        return enemies
            .Where(e => Vector3.Distance(towerPosition, e.transform.position) <= range)
            .OrderByDescending(e =>
            {
                var eb = e.GetComponent<EnemyBase>();
                return eb != null ? eb.WaypointIndex : 0;
            })
            .FirstOrDefault()?.transform;
    }
}