using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CommandManager : PersistentSingleton<CommandManager>
{
    private readonly Stack<ICommand> undoStack = new();
    private readonly Stack<ICommand> redoStack = new();

    [SerializeField] private int maxHistorySize = 50;

    public bool CanUndo => undoStack.Count > 0;
    public bool CanRedo => redoStack.Count > 0;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (InputReader.Instance != null)
        {
            InputReader.Instance.OnUndoPerformed += Undo;
            InputReader.Instance.OnRedoPerformed += Redo;
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (InputReader.Instance != null)
        {
            InputReader.Instance.OnUndoPerformed -= Undo;
            InputReader.Instance.OnRedoPerformed -= Redo;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ClearHistory();
    }

    public void Execute(ICommand command)
    {
        command.Execute();
        undoStack.Push(command);
        redoStack.Clear();

        if (undoStack.Count > maxHistorySize)
        {
            var tmp = new Stack<ICommand>(undoStack);
            undoStack.Clear();
            int count = 0;
            foreach (var cmd in tmp)
            {
                if (count++ >= maxHistorySize) break;
                undoStack.Push(cmd);
            }
        }

        Debug.Log($"[CommandManager] Execute: {command.Description}");
    }

    public void Undo()
    {
        if (!CanUndo) { Debug.Log("[CommandManager] Nothing to undo"); return; }

        var command = undoStack.Pop();
        command.Undo();
        redoStack.Push(command);

        Debug.Log($"[CommandManager] Undo: {command.Description}");
    }

    public void Redo()
    {
        if (!CanRedo) { Debug.Log("[CommandManager] Nothing to redo"); return; }

        var command = redoStack.Pop();
        command.Execute();
        undoStack.Push(command);

        Debug.Log($"[CommandManager] Redo: {command.Description}");
    }

    public void ClearHistory()
    {
        undoStack.Clear();
        redoStack.Clear();
    }
}