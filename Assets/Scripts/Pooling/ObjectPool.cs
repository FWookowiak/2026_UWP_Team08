using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private readonly T prefab;
    private readonly Transform parent;
    private readonly Queue<T> available = new();
    private readonly HashSet<T> active = new();

    public int CountAvailable => available.Count;
    public int CountActive => active.Count;

    public ObjectPool(T prefab, int initialSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < initialSize; i++)
            CreateNew();
    }

    private T CreateNew()
    {
        T instance = Object.Instantiate(prefab, parent);
        instance.gameObject.SetActive(false);
        available.Enqueue(instance);
        return instance;
    }

    public T Get(Vector3 position, Quaternion rotation)
    {
        T instance;
        if (available.Count == 0)
            instance = CreateNew();

        instance = available.Dequeue();
        instance.transform.SetPositionAndRotation(position, rotation);
        instance.gameObject.SetActive(true);
        active.Add(instance);

        return instance;
    }

    public void Return(T instance)
    {
        if (instance == null) return;
        if (!active.Contains(instance)) return;

        instance.gameObject.SetActive(false);
        active.Remove(instance);
        available.Enqueue(instance);
    }

    public void ReturnAll()
    {
        foreach (var instance in active)
        {
            if (instance != null)
                instance.gameObject.SetActive(false);
            available.Enqueue(instance);
        }
        active.Clear();
    }
}