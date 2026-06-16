using UnityEngine;

public class SlowingTower : TowerBase
{
    [Header("Slowing")]
    [SerializeField] private float slowAmount = 0.4f;
    [SerializeField] private float slowDuration = 2f;

    public float SlowAmount => slowAmount;
    public float SlowDuration => slowDuration;
}