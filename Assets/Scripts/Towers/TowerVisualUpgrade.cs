using UnityEngine;

/// <summary>
/// Komponent na prefabie wieży — zarządza wizualną ewolucją po ulepszeniach.
/// Obserwuje GameEvents.OnTowerUpgraded i podmienia mesh / aktywuje efekty
/// w zależności od poziomu ulepszenia.
/// 
/// Setup w Inspectorze:
/// - upgradeStages: tablica wariantów wizualnych (level 0 = bazowy)
/// - każdy stage to GameObject (child wieży) — aktywny jest tylko jeden naraz
/// </summary>
[RequireComponent(typeof(TowerBase))]
public class TowerVisualUpgrade : MonoBehaviour
{
    [System.Serializable]
    public class UpgradeStage
    {
        public string stageName;
        public GameObject visualRoot;       // child z odpowiednim mesh
        public GameObject upgradeEffect;    // particle do zagrania przy ulepszeniu
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
        // Pokaż tylko stage 0 (bazowy)
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
            // Mały efekt nawet bez zmiany stage — żeby było czuć ulepszenie
            PlayUpgradeFeedback();
        }
    }

    private void ApplyStage(int stageIndex)
    {
        if (upgradeStages == null || upgradeStages.Length == 0) return;
        stageIndex = Mathf.Clamp(stageIndex, 0, upgradeStages.Length - 1);

        // Wyłącz wszystkie wizualne warianty
        for (int i = 0; i < upgradeStages.Length; i++)
        {
            if (upgradeStages[i].visualRoot != null)
                upgradeStages[i].visualRoot.SetActive(i == stageIndex);
        }

        currentStage = stageIndex;

        // Particle effect przy zmianie stage
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