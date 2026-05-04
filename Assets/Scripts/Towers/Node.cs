using UnityEngine;

public class Node : MonoBehaviour
{
    public Color hoverColor = Color.cyan;
    public Color cantBuildColor = Color.red;
    public Color occupiedColor = Color.yellow;
    public Vector3 positionOffset;

    [Header("State")]
    public GameObject tower;
    public TowerConfig towerConfig;

    private Renderer rend;
    private Color startColor;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
            startColor = rend.material.color;
    }

    public Vector3 GetBuildPosition()
    {
        return transform.position + positionOffset;
    }
    
    public void HandleClick()
    {
        if (tower != null)
        {
            TowerBase tb = tower.GetComponent<TowerBase>();
            if (tb != null)
                GameEvents.TowerSelected(tb, this);
            return;
        }

        if (BuildManager.Instance == null || !BuildManager.Instance.CanBuild)
            return;

        BuildManager.Instance.BuildTowerOn(this);
    }
    public void OnHoverEnter()
    {
        if (rend == null) return;

        if (tower != null)
        {
            rend.material.color = occupiedColor;
            return;
        }

        if (BuildManager.Instance == null || !BuildManager.Instance.CanBuild) return;

        rend.material.color = BuildManager.Instance.HasMoney ? hoverColor : cantBuildColor;
    }

    public void OnHoverExit()
    {
        if (rend != null)
            rend.material.color = startColor;
    }
}