using UnityEngine;

[System.Serializable]
public class TutorialStep
{
    public string stepId;
    public string title;
    [TextArea(3, 5)]
    public string description;
    public string highlightTargetName; 
    public bool requiresAction;       
    public string requiredActionId;   
}
