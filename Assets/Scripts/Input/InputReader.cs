using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    public static InputReader Instance { get; private set; }

    [Header("Input")]
    [SerializeField] private InputActionAsset inputActions;

    [Header("Raycast")]
    [SerializeField] private LayerMask nodeLayer = ~0;
    [SerializeField] private float raycastDistance = 100f;

    private InputAction clickAction;
    private InputAction positionAction;
    private InputAction cancelAction;
    private InputAction startWaveAction;
    private InputAction undoAction;
    private InputAction redoAction;

    private Camera mainCamera;
    private Node hoveredNode;
    
    private bool pendingClick = false;
    private Vector2 pendingClickPos;

    public event Action<Vector2> OnPointerClick;
    public event Action OnCancelPerformed;
    public event Action OnStartWavePerformed;
    public event Action OnUndoPerformed;
    public event Action OnRedoPerformed;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        var gameplay = inputActions.FindActionMap("Gameplay", throwIfNotFound: true);
        clickAction     = gameplay.FindAction("Click");
        positionAction  = gameplay.FindAction("Position");
        cancelAction    = gameplay.FindAction("Cancel");
        startWaveAction = gameplay.FindAction("StartWave");
        undoAction      = gameplay.FindAction("Undo");
        redoAction      = gameplay.FindAction("Redo");

        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        clickAction.performed     += HandleClick;
        cancelAction.performed    += HandleCancel;
        startWaveAction.performed += HandleStartWave;
        undoAction.performed      += HandleUndo;
        redoAction.performed      += HandleRedo;

        inputActions.Enable();
    }

    private void OnDisable()
    {
        clickAction.performed     -= HandleClick;
        cancelAction.performed    -= HandleCancel;
        startWaveAction.performed -= HandleStartWave;
        undoAction.performed      -= HandleUndo;
        redoAction.performed      -= HandleRedo;

        inputActions.Disable();
    }

    private void Update()
    {
        UpdateHover();
        ProcessPendingClick(); 
    }

    private void UpdateHover()
    {
        if (mainCamera == null) return;
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            ClearHover();
            return;
        }

        Vector2 screenPos = positionAction.ReadValue<Vector2>();
        Ray ray = mainCamera.ScreenPointToRay(screenPos);

        Node currentNode = null;
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, nodeLayer))
            currentNode = hit.collider.GetComponentInParent<Node>();

        if (currentNode != hoveredNode)
        {
            if (hoveredNode != null) hoveredNode.OnHoverExit();
            if (currentNode != null) currentNode.OnHoverEnter();
            hoveredNode = currentNode;
        }
    }

    private void ClearHover()
    {
        if (hoveredNode != null)
        {
            hoveredNode.OnHoverExit();
            hoveredNode = null;
        }
    }
    private void HandleClick(InputAction.CallbackContext ctx)
    {
        pendingClick    = true;
        pendingClickPos = positionAction.ReadValue<Vector2>();
    }
    
    private void ProcessPendingClick()
    {
        if (!pendingClick) return;
        pendingClick = false;

        OnPointerClick?.Invoke(pendingClickPos);
        
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (mainCamera == null) return;

        Ray ray = mainCamera.ScreenPointToRay(pendingClickPos);
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, nodeLayer))
        {
            Node node = hit.collider.GetComponentInParent<Node>();
            node?.HandleClick();
        }
    }
    

    private void HandleCancel(InputAction.CallbackContext ctx)    => OnCancelPerformed?.Invoke();
    private void HandleStartWave(InputAction.CallbackContext ctx) => OnStartWavePerformed?.Invoke();
    private void HandleUndo(InputAction.CallbackContext ctx)      => OnUndoPerformed?.Invoke();
    private void HandleRedo(InputAction.CallbackContext ctx)      => OnRedoPerformed?.Invoke();
}