using System.Linq;
using UnityEngine;

public class ClosestTargetingStrategy : ITargetingStrategy
{
    public string Name => "Najbliższy";

    public Transform SelectTarget(Vector3 towerPosition, float range, string enemyTag)
    {
        var enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        return enemies
            .Where(e => Vector3.Distance(towerPosition, e.transform.position) <= range)
            .OrderBy(e => Vector3.Distance(towerPosition, e.transform.position))
            .FirstOrDefault()?.transform;
    }
}