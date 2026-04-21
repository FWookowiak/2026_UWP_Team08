using UnityEngine;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private TutorialView tutorialView;
    [SerializeField] private List<TutorialStep> steps = new List<TutorialStep>();

    private int currentStepIndex = -1;
    private bool tutorialActive = false;

    private const string TUTORIAL_DONE_KEY = "TutorialCompleted";

    private void Start()
    {
        DefineSteps();

        tutorialView.OnNextClicked += NextStep;
        tutorialView.OnSkipClicked += SkipTutorial;

        // Uruchom tutorial tylko raz
        if (PlayerPrefs.GetInt(TUTORIAL_DONE_KEY, 0) == 0)
        {
            StartTutorial();
        }
    }

    private void DefineSteps()
    {
        steps.Clear();

        steps.Add(new TutorialStep
        {
            stepId = "welcome",
            title = "Witaj w Tower Defense!",
            description = "Twoim celem jest obrona bazy przed falami wrogów. Stawiaj wieże i ulepszaj je!",
            requiresAction = false
        });

        steps.Add(new TutorialStep
        {
            stepId = "place_tower",
            title = "Rozstawianie wież",
            description = "Kliknij na pole na mapie, aby postawić wieżę. Potrzebujesz wystarczająco dużo złota!",
            highlightTargetName = "TowerButton1",
            requiresAction = false
        });

        steps.Add(new TutorialStep
        {
            stepId = "enemy_info",
            title = "Wrogowie atakują!",
            description = "Wrogowie poruszają się po ścieżce. Każdy typ (T1, T2, T3) ma inne HP i szybkość. Pasek zdrowia nad wrogiem pokazuje jego kondycję.",
            requiresAction = false
        });

        steps.Add(new TutorialStep
        {
            stepId = "start_wave",
            title = "Rozpocznij falę",
            description = "Naciśnij SPACJĘ aby rozpocząć falę wrogów. Możesz sprawdzić skład następnej fali w panelu podglądu.",
            highlightTargetName = "WavePreviewPanel",
            requiresAction = false
        });

        steps.Add(new TutorialStep
        {
            stepId = "upgrade_tower",
            title = "Ulepszanie wież",
            description = "Kliknij na postawioną wieżę, aby otworzyć panel ulepszeń. Możesz zwiększyć zasięg, szybkość lub obrażenia.",
            requiresAction = false
        });

        steps.Add(new TutorialStep
        {
            stepId = "strategy",
            title = "Strategia celowania",
            description = "W panelu wieży możesz zmienić strategię celowania: Najbliższy, Najsilniejszy, Najsłabszy lub Pierwszy wróg na ścieżce.",
            requiresAction = false
        });
        
        steps.Add(new TutorialStep
        {
            stepId = "sell_tower",
            title = "Sprzedawanie wież",
            description = "Kliknij na wieżę, a następnie naciśnij przycisk 'Sprzedaj' w panelu. " +
                          "Otrzymasz 50% kosztów budowy z powrotem. " +
                          "Sprzedawaj wieże, które nie są już potrzebne, aby odzyskać złoto!",
            highlightTargetName = "SellButton",
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

        // Highlight
        if (!string.IsNullOrEmpty(step.highlightTargetName))
        {
            GameObject target = GameObject.Find(step.highlightTargetName);
            tutorialView.HighlightElement(target);
        }
        else
        {
            tutorialView.ClearHighlight();
        }
    }

    public void SkipTutorial()
    {
        CompleteTutorial();
    }

    private void CompleteTutorial()
    {
        tutorialActive = false;
        tutorialView.HidePopup();
        PlayerPrefs.SetInt(TUTORIAL_DONE_KEY, 1);
        PlayerPrefs.Save();
        Debug.Log("Tutorial zakończony!");
    }

    // Wywoływane przez inne systemy gdy gracz wykonał akcję
    public void NotifyAction(string actionId)
    {
        if (!tutorialActive) return;
        if (currentStepIndex < 0 || currentStepIndex >= steps.Count) return;

        TutorialStep step = steps[currentStepIndex];
        if (step.requiresAction && step.requiredActionId == actionId)
        {
            NextStep();
        }
    }

    private void OnDestroy()
    {
        if (tutorialView != null)
        {
            tutorialView.OnNextClicked -= NextStep;
            tutorialView.OnSkipClicked -= SkipTutorial;
        }
    }
}
