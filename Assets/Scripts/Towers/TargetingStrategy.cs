using UnityEngine;
using System.Linq;

public enum TargetingMode
{
    Closest,
    Strongest,
    Weakest,
    First
}

public static class TargetingStrategy
{
    public static Transform GetTarget(
        Vector3 towerPos, float range, string enemyTag, TargetingMode mode)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        var inRange = enemies
            .Where(e => Vector3.Distance(towerPos, e.transform.position) <= range)
            .ToArray();

        if (inRange.Length == 0) return null;

        switch (mode)
        {
            case TargetingMode.Closest:
                return inRange
                    .OrderBy(e => Vector3.Distance(towerPos, e.transform.position))
                    .First().transform;

            case TargetingMode.Strongest:
                return inRange
                    .OrderByDescending(e =>
                    {
                        var eb = e.GetComponent<EnemyBase>();
                        return eb != null ? eb.CurrentHealth : 0;
                    })
                    .First().transform;

            case TargetingMode.Weakest:
                return inRange
                    .OrderBy(e =>
                    {
                        var eb = e.GetComponent<EnemyBase>();
                        return eb != null ? eb.CurrentHealth : float.MaxValue;
                    })
                    .First().transform;

            case TargetingMode.First:
                return inRange
                    .OrderByDescending(e =>
                    {
                        var eb = e.GetComponent<EnemyBase>();
                        return eb != null ? eb.WaypointIndex : 0;
                    })
                    .First().transform;

            default:
                return inRange[0].transform;
        }
    }
}
