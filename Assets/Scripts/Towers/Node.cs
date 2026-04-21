using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public Color hoverColor = Color.cyan;
    public Color cantBuildColor = Color.red;
    public Color occupiedColor = Color.yellow;
    public Vector3 positionOffset;

    [Header("State")]
    public GameObject tower;
    public TowerConfig towerConfig; // zapamiętaj config (do sprzedaży)

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

    private void OnMouseDown()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        // Jeśli wieża stoi — zaznacz ją (do panelu upgrade / sprzedaży)
        if (tower != null)
        {
            TowerBase tb = tower.GetComponent<TowerBase>();
            if (tb != null)
            {
                // Znajdź TowerUpgradePresenter i zaznacz
                var presenter = FindObjectOfType<TowerUpgradePresenter>();
                if (presenter != null)
                    presenter.SelectTower(tb, this);
            }
            return;
        }

        if (!BuildManager.Instance.CanBuild)
            return;

        BuildManager.Instance.BuildTowerOn(this);
    }

    private void OnMouseEnter()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (rend == null) return;

        if (tower != null)
        {
            rend.material.color = occupiedColor;
            return;
        }

        if (!BuildManager.Instance.CanBuild) return;

        rend.material.color = BuildManager.Instance.HasMoney 
            ? hoverColor 
            : cantBuildColor;
    }

    private void OnMouseExit()
    {
        if (rend != null)
            rend.material.color = startColor;
    }
}