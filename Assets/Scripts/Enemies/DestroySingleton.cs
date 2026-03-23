using UnityEngine;

public abstract class DestroySingleton<T> : MonoBehaviour where T : Component
{
    public static T Instance { get; private set; }

    protected virtual void Awake() {
        if (Instance == null) {
            Instance = this as T;
        }
        else if (Instance != this as T)
        {
            Debug.LogWarning($"Znaleziono duplikat Singletona {typeof(T)}! Niszczę obiekt: {gameObject.name}");
            Destroy(gameObject);
        }
    }
}