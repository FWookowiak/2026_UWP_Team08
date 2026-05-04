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
        GameEvents.OnTowerSold  += HandleTowerSold;
        GameEvents.OnEnemyKilled += HandleEnemyKilled;
        GameEvents.OnWaveStarted += HandleWaveStarted;
    }

    private void OnDisable()
    {
        GameEvents.OnTowerBuilt -= HandleTowerBuilt;
        GameEvents.OnTowerSold  -= HandleTowerSold;
        GameEvents.OnEnemyKilled -= HandleEnemyKilled;
        GameEvents.OnWaveStarted -= HandleWaveStarted;
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
            description = "Twoim celem jest obrona bazy przed falami wrogów.",
            requiresAction = false
        });

        steps.Add(new TutorialStep
        {
            stepId = "select_tower",
            title = "Wybór wieży",
            description = "Najpierw wybierz typ wieży klikając przycisk na dolnym pasku.",
            highlightTargetName = "TowerBtn1",
            requiresAction = false
        });

        // KLUCZOWY KROK — czeka na faktyczne postawienie wieży
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
            stepId = "start_wave_hint",
            title = "Naciśnij Spację",
            description = "Gdy będziesz gotów, naciśnij Spację, aby rozpocząć falę wrogów.",
            requiresAction = false
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

    private void OnDestroy()
    {
        if (tutorialView != null)
        {
            tutorialView.OnNextClicked -= NextStep;
            tutorialView.OnSkipClicked -= SkipTutorial;
        }
    }
}