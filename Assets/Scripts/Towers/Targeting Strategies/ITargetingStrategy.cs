using UnityEngine;

public interface ITargetingStrategy
{
    string Name { get; }
    Transform SelectTarget(Vector3 towerPosition, float range, string enemyTag);
}