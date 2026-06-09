using UnityEngine;

public class SlowingTower : TowerBase
{
    [Header("Slowing")]
    [SerializeField] private float slowAmount = 0.4f;     // mnożnik prędkości (0.4 = 40% normalnej)
    [SerializeField] private float slowDuration = 2f;     // ile sekund trwa spowolnienie

    public float SlowAmount => slowAmount;
    public float SlowDuration => slowDuration;
}