using UnityEngine;

public class TutorialNodeHighlight : MonoBehaviour
{
    [SerializeField] private GameObject highlightPrefab; 
    [SerializeField] private float bobSpeed = 2f;
    [SerializeField] private float bobAmount = 0.3f;
    [SerializeField] private float yOffset = 1.5f;

    private GameObject activeHighlight;
    private Vector3 basePosition;

    public void HighlightNode(Transform node)
    {
        ClearHighlight();
        if (highlightPrefab == null || node == null) return;

        basePosition = node.position + Vector3.up * yOffset;
        activeHighlight = Instantiate(highlightPrefab, basePosition, Quaternion.identity);
    }

    public void ClearHighlight()
    {
        if (activeHighlight != null)
            Destroy(activeHighlight);
        activeHighlight = null;
    }

    private void Update()
    {
        if (activeHighlight == null) return;

        float bob = Mathf.Sin(Time.time * bobSpeed) * bobAmount;
        activeHighlight.transform.position = basePosition + Vector3.up * bob;
        activeHighlight.transform.Rotate(Vector3.up, 60f * Time.deltaTime);
    }
}