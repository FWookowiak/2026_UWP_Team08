using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public Color hoverColor;
    public Vector3 positionOffset;

    [Header("Optional")]
    public GameObject tower;

    private Renderer rend;
    private Color startColor;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            startColor = rend.material.color;
        }
    }

    public Vector3 GetBuildPosition()
    {
        return transform.position + positionOffset;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (tower != null)
        {
            Debug.Log("Can't build there! - TODO: Display on screen.");
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

        if (!BuildManager.Instance.CanBuild)
            return;

        if (BuildManager.Instance.HasMoney && rend != null)
        {
            rend.material.color = hoverColor;
        }
        else if (rend != null)
        {
            rend.material.color = Color.red;
        }
    }

    private void OnMouseExit()
    {
        if (rend != null)
        {
            rend.material.color = startColor;
        }
    }
}
