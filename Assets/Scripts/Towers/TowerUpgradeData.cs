using UnityEngine;

[CreateAssetMenu(menuName = "Tower/TowerUpgradeData")]
public class TowerUpgradeData : ScriptableObject
{
    public string upgradeName;
    public int cost;

    [Header("Bonusy")]
    public float rangeBonus;
    public float fireRateBonus;
    public float damageBonus;

    public int maxLevel = 3;
}
