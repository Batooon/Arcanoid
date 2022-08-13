using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private readonly List<T> _pooledObjects = new List<T>();
    private T _pooledPrefab;
    private int _objectsAmount;
    
    public ObjectPool(T objectToPool, int objectsAmount = 50)
    {
        _pooledPrefab = objectToPool;
        _objectsAmount = objectsAmount;

        var generatedObjects = GenerateObjects(objectsAmount);
        _pooledObjects.AddRange(generatedObjects);
    }

    public IEnumerable<T> GenerateObjects(int amount)
    {
        var generatedObjects = new T[amount];
        for (var i = 0; i < amount; i++)
        {
            generatedObjects[i] = GenerateObject();
        }

        return generatedObjects;
    }

    private T GenerateObject()
    {
        var generatedObject = Object.Instantiate(_pooledPrefab);
        generatedObject.gameObject.SetActive(false);
        return generatedObject;
    }

    public T GetAvailableObject()
    {
        foreach (var obj in GetActiveObjects())
        {
            return obj;
        }

        var newObj = GenerateObject();
        _pooledObjects.Add(newObj);
        return newObj;
    }

    public IEnumerable<T> GetActiveObjects()
    {
        return _pooledObjects.Where(obj => obj.gameObject.activeInHierarchy);
    }
}