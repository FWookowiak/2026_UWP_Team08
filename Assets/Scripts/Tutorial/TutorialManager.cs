using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private TutorialView tutorialView;
    [SerializeField] private TutorialNodeHighlight nodeHighlight;
    [SerializeField] private Transform tutorialTargetNode;

    [SerializeField] private List<TutorialStep> steps = new();

    private int currentStepIndex = -1;
    private bool tutorialActive = false;

    private const string TUTORIAL_DONE_KEY = "TutorialCompleted";

    private void OnEnable()
    {
        GameEvents.OnTowerBuilt += HandleTowerBuilt;
        GameEvents.OnTowerSold += HandleTowerSold;
        GameEvents.OnEnemyKilled += HandleEnemyKilled;
        GameEvents.OnWaveStarted += HandleWaveStarted;
        GameEvents.OnTowerTypeSelected += HandleTowerTypeSelected;
        GameEvents.OnTowerSelected += HandleTowerSelected;
        GameEvents.OnTowerUpgraded += HandleTowerUpgraded;
        GameEvents.OnStrategyChanged += HandleStrategyChanged;
    }

    private void OnDisable()
    {
        GameEvents.OnTowerBuilt -= HandleTowerBuilt;
        GameEvents.OnTowerSold -= HandleTowerSold;
        GameEvents.OnEnemyKilled -= HandleEnemyKilled;
        GameEvents.OnWaveStarted -= HandleWaveStarted;
        GameEvents.OnTowerTypeSelected -= HandleTowerTypeSelected;
        GameEvents.OnTowerSelected -= HandleTowerSelected;
        GameEvents.OnTowerUpgraded -= HandleTowerUpgraded;
        GameEvents.OnStrategyChanged -= HandleStrategyChanged;
    }

    private void Start()
    {
        DefineSteps();
        tutorialView.OnNextClicked += NextStep;
        tutorialView.OnSkipClicked += SkipTutorial;

        if (PlayerPrefs.GetInt(TUTORIAL_DONE_KEY, 0) == 0)
            StartTutorial();
    }

    private void DefineSteps()
    {
        steps.Clear();

        steps.Add(new TutorialStep
        {
            stepId = "welcome",
            title = "Witaj w Tower Defense!",
            description = "Twoim celem jest obrona bazy przed falami wrogów. Naciśnij 'Dalej', aby kontynuować.",
            requiresAction = false
        });

        steps.Add(new TutorialStep
        {
            stepId = "select_tower",
            title = "Wybierz wieżę",
            description = "Kliknij przycisk wieży na dolnym pasku, aby wybrać typ wieży do postawienia.",
            highlightTargetName = "TowerBtn1",
            requiresAction = true,
            requiredActionId = "tower_type_selected"
        });

        steps.Add(new TutorialStep
        {
            stepId = "place_tower",
            title = "Postaw wieżę",
            description = "Kliknij na podświetlone pole, aby postawić wieżę.",
            requiresAction = true,
            requiredActionId = "tower_built"
        });

        steps.Add(new TutorialStep
        {
            stepId = "tower_placed_confirmation",
            title = "Świetnie!",
            description = "Wieża postawiona. Możesz stawiać kolejne wieże w wolnych polach.",
            requiresAction = false
        });
        
        steps.Add(new TutorialStep
        {
            stepId = "enemy_attack_info",
            title = "Atak przeciwników",
            description = "Wrogowie poruszają się po ścieżce w stronę twojej bazy. " +
                          "Każdy wróg, który dotrze do bazy, zadaje jej obrażenia. " +
                          "Gdy zdrowie bazy spadnie do zera — przegrywasz! " +
                          "Twoje wieże muszą zabić wrogów zanim dotrą do celu.",
            requiresAction = false
        });

        steps.Add(new TutorialStep
        {
            stepId = "select_built_tower",
            title = "Zarządzanie wieżą",
            description = "Kliknij na postawioną wieżę, aby otworzyć panel zarządzania.",
            requiresAction = true,
            requiredActionId = "tower_selected"
        });

        steps.Add(new TutorialStep
        {
            stepId = "upgrade_tower",
            title = "Ulepsz wieżę",
            description = "Naciśnij przycisk ulepszenia w panelu. Ulepszenie kosztuje złoto i zwiększa statystyki wieży.",
            highlightTargetName = "UpgradeBtn1",
            requiresAction = true,
            requiredActionId = "tower_upgraded"
        });

        steps.Add(new TutorialStep
        {
            stepId = "change_strategy",
            title = "Zmień strategię",
            description = "Wieża może celować w różny sposób. Wybierz jedną ze strategii: najbliższy, najsilniejszy, najsłabszy, pierwszy na ścieżce.",
            highlightTargetName = "StrategyBtn_Strongest",
            requiresAction = true,
            requiredActionId = "strategy_changed"
        });

        steps.Add(new TutorialStep
        {
            stepId = "start_wave_hint",
            title = "Naciśnij Spację",
            description = "Gdy będziesz gotów, naciśnij Spację, aby rozpocząć falę wrogów.",
            requiresAction = true,
            requiredActionId = "wave_started"
        });
    }

    public void StartTutorial()
    {
        tutorialActive = true;
        currentStepIndex = -1;
        tutorialView.ShowSkipButton(true);
        NextStep();
    }

    public void NextStep()
    {
        currentStepIndex++;

        if (currentStepIndex >= steps.Count)
        {
            CompleteTutorial();
            return;
        }

        TutorialStep step = steps[currentStepIndex];
        tutorialView.ShowStep(step.title, step.description);
        tutorialView.ShowNextButton(!step.requiresAction);

        if (!string.IsNullOrEmpty(step.highlightTargetName))
        {
            GameObject target = GameObject.Find(step.highlightTargetName);
            tutorialView.HighlightElement(target);
        }
        else
        {
            tutorialView.ClearHighlight();
        }

        if (step.stepId == "place_tower" && nodeHighlight != null && tutorialTargetNode != null)
            nodeHighlight.HighlightNode(tutorialTargetNode);
        else if (nodeHighlight != null)
            nodeHighlight.ClearHighlight();
    }

    public void SkipTutorial() => CompleteTutorial();

    private void CompleteTutorial()
    {
        tutorialActive = false;
        tutorialView.HidePopup();
        if (nodeHighlight != null) nodeHighlight.ClearHighlight();
        PlayerPrefs.SetInt(TUTORIAL_DONE_KEY, 1);
        PlayerPrefs.Save();
    }

    public void NotifyAction(string actionId)
    {
        if (!tutorialActive) return;
        if (currentStepIndex < 0 || currentStepIndex >= steps.Count) return;

        TutorialStep step = steps[currentStepIndex];
        if (step.requiresAction && step.requiredActionId == actionId)
            NextStep();
    }

    private void HandleTowerBuilt(GameObject t, Node n, int c) => NotifyAction("tower_built");
    private void HandleTowerSold(GameObject t, Node n, int r) => NotifyAction("tower_sold");
    private void HandleEnemyKilled(EnemyBase e, int g) => NotifyAction("enemy_killed");
    private void HandleWaveStarted(int cur, int total) => NotifyAction("wave_started");
    private void HandleTowerTypeSelected(TowerConfig c) => NotifyAction("tower_type_selected");
    private void HandleTowerSelected(TowerBase t, Node n) => NotifyAction("tower_selected");
    private void HandleTowerUpgraded(TowerBase t, TowerUpgradeData d) => NotifyAction("tower_upgraded");
    private void HandleStrategyChanged(TowerBase t, TargetingMode m) => NotifyAction("strategy_changed");

    private void OnDestroy()
    {
        if (tutorialView != null)
        {
            tutorialView.OnNextClicked -= NextStep;
            tutorialView.OnSkipClicked -= SkipTutorial;
        }
    }
}