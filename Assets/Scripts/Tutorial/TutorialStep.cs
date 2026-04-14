using UnityEngine;

[System.Serializable]
public class TutorialStep
{
    public string stepId;
    public string title;
    [TextArea(3, 5)]
    public string description;
    public string highlightTargetName; // nazwa GameObject do podświetlenia
    public bool requiresAction;        // czy czeka na akcję gracza
    public string requiredActionId;    // identyfikator akcji
}
