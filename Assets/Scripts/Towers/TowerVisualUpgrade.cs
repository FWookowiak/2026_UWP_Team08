using UnityEngine;

[RequireComponent(typeof(TowerBase))]
public class TowerVisualUpgrade : MonoBehaviour
{
    [System.Serializable]
    public class UpgradeStage
    {
        public string stageName;
        public GameObject visualRoot;
        public GameObject upgradeEffect;
        [Tooltip("Po ilu łącznych ulepszeniach aktywować ten stage")]
        public int requiredUpgrades;
    }

    [SerializeField] private UpgradeStage[] upgradeStages;
    [SerializeField] private float scaleOnUpgrade = 1.1f;

    private TowerBase tower;
    private int totalUpgrades = 0;
    private int currentStage = 0;

    private void Awake()
    {
        tower = GetComponent<TowerBase>();
    }

    private void Start()
    {
        ApplyStage(0);
    }

    private void OnEnable()
    {
        GameEvents.OnTowerUpgraded += HandleUpgrade;
    }

    private void OnDisable()
    {
        GameEvents.OnTowerUpgraded -= HandleUpgrade;
    }

    private void HandleUpgrade(TowerBase upgradedTower, TowerUpgradeData data)
    {
        // Tylko jeśli to ta wieża
        if (upgradedTower != tower) return;

        totalUpgrades++;

        // Sprawdź czy odblokować nowy stage
        int newStage = currentStage;
        for (int i = upgradeStages.Length - 1; i >= 0; i--)
        {
            if (totalUpgrades >= upgradeStages[i].requiredUpgrades)
            {
                newStage = i;
                break;
            }
        }

        if (newStage != currentStage)
        {
            ApplyStage(newStage);
        }
        else
        {
            PlayUpgradeFeedback();
        }
    }

    private void ApplyStage(int stageIndex)
    {
        if (upgradeStages == null || upgradeStages.Length == 0) return;
        stageIndex = Mathf.Clamp(stageIndex, 0, upgradeStages.Length - 1);

        for (int i = 0; i < upgradeStages.Length; i++)
        {
            if (upgradeStages[i].visualRoot != null)
                upgradeStages[i].visualRoot.SetActive(i == stageIndex);
        }

        currentStage = stageIndex;

        var effect = upgradeStages[stageIndex].upgradeEffect;
        if (effect != null)
        {
            GameObject spawned = Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(spawned, 3f);
        }

        // Mały scale boost dla feedbacku
        StartCoroutine(ScalePunch());
    }

    private void PlayUpgradeFeedback()
    {
        StartCoroutine(ScalePunch());
    }

    private System.Collections.IEnumerator ScalePunch()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * scaleOnUpgrade;
        float duration = 0.15f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, targetScale, t / duration);
            yield return null;
        }

        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(targetScale, originalScale, t / duration);
            yield return null;
        }

        transform.localScale = originalScale;
    }
}