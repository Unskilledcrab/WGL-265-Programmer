using System.Collections.Generic;
using UnityEngine;

public static class Pool
{
    private static Dictionary<string, Queue<Component>> _pools = new();
    private static int _defaultSize = 10;
    private static int _growthRate = 5;

    /// <summary>
    /// Get the prefab from a pool, if there is not a pool already it will create it
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public static K GetFromPool<K>(this K prefab) where K : Component
    {
        var key = prefab.GetType().Name;
        var poolExists = _pools.TryGetValue(key, out var pool);
        if (poolExists)
        {
            if (pool.TryDequeue(out var obj))
                return ConvertObject<K>(obj);

            AddToPool(prefab, pool, _growthRate);
            return ConvertObject<K>(pool.Dequeue());
        }

        pool = new Queue<Component>();
        AddToPool(prefab, pool, _defaultSize);
        _pools.Add(key, pool);
        return ConvertObject<K>(pool.Dequeue());
    }

    private static K ConvertObject<K>(Component obj) where K : Component
    {
        if (obj == null)
            return default;
        return (K)obj;
    }

    /// <summary>
    /// Returns the prefab to it's pool
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <param name="prefab"></param>
    public static void ReturnToPool<K>(this K prefab) where K : Component
    {
        var key = prefab.GetType().Name;
        if (_pools.TryGetValue(key, out var pool))
        {
            prefab.gameObject.SetActive(false);
            pool.Enqueue(prefab);
            return;
        }
        Debug.LogWarning($"Pool [{key}]: Does not exist for {prefab.name}");
    }

    /// <summary>
    /// Will destory all prefabs in the game to free up resources
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <param name="prefab"></param>
    public static void CleanPool<K>(this K prefab) where K : Component
    {
        var key = prefab.GetType().Name;
        if (_pools.TryGetValue(key, out var pool))
        {
            while (pool.Count > _defaultSize)
            {
                Object.Destroy(pool.Dequeue());
            }
        }
    }

    private static void AddToPool<K>(K prefab, Queue<Component> pool, int size) where K : Component
    {
        for (int i = 0; i < size; i++)
        {
            var component = Object.Instantiate(prefab);
            component.gameObject.SetActive(false);
            pool.Enqueue(component);
        }
    }
}