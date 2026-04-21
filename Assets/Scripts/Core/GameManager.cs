using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PersistentSingleton<T> : MonoBehaviour where T : Component
{
    public static T Instance { get; private set; }
    protected virtual void Awake() {
        if (Instance == null) {
            Instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if(Instance != this as T) {
            Destroy(gameObject);
        }
    }
}
public enum GameState
{
    MainMenu,
    BuildPhase,
    DefensePhase,
    Victory,
    Defeat
}

public class GameManager : PersistentSingleton<GameManager> {
    [Header("Game State")]
    [SerializeField] private GameState currentState;
    public GameState CurrentState => currentState;
    protected override void Awake()
    { base.Awake(); }
    private void Start() {
        ChangeState(GameState.BuildPhase);
    }
    private void Update() {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && currentState == GameState.BuildPhase)
        {
            ChangeState(GameState.DefensePhase);
        }
    }

    public void ChangeState(GameState newState) {
        currentState = newState;
        switch (currentState)
        {
            case GameState.BuildPhase:
                HandleBuildPhase();
                break;
            case GameState.DefensePhase:
                HandleDefensePhase();
                break;
            case GameState.Victory:
                HandleVictory();
                break;
            case GameState.Defeat:
                HandleDefeat();
                break;
        }
        GameEvents.GameStateChanged(currentState);
    }

    public void HandleBuildPhase() {
        Debug.Log("Build Phase");
    }
    public void HandleDefensePhase() {
        Debug.Log("Defense Phase");
        WaveManager.Instance.StartNextRound();
    }
    public void HandleVictory() {
        Debug.Log("Victory");
    }
    public void HandleDefeat() {
        Debug.Log("Defeat");
    }

    public void TriggerVictory() {
        ChangeState(GameState.Victory);
    }
    public void TriggerDefeat() {
        ChangeState(GameState.Defeat);
    }
    
}
