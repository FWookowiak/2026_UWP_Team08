using UnityEngine;

[CreateAssetMenu(menuName = "Wave/WaveData")]
public class WaveData : ScriptableObject{
    public GameObject enemyPrefab;
    public int count;
    public float spawnInterval = 1f;
    public float delayBeforeNextGroup;
}