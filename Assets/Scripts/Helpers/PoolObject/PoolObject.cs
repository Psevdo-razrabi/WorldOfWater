using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using Helpers.PoolObject;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class PoolObject
{
    private Dictionary<PoolKey, Queue<ObjectInPool>> _poolObject;
    private readonly Dictionary<string, PoolKey> _keyRegistry = new();
    private DiContainer _container;

    public bool AutoExpandPool { get; private set; } = true;
        
    [Inject]
    public void Construct(DiContainer container)
    {
        _container = container ?? throw new ArgumentNullException($"{nameof(container)} is null");
            
        _poolObject = new Dictionary<PoolKey, Queue<ObjectInPool>>();
    }
    
    public PoolKey GetOrRegisterKey(string keyName)
    {
        Preconditions.CheckNotNull(keyName);
    
        if (_keyRegistry.TryGetValue(keyName, out PoolKey key) == false)
        {
            key = new PoolKey(keyName);
            _keyRegistry[keyName] = key;
        }
    
        return key;
    }

    public void AddElementsInPool(PoolKey keyObjectInPool, GameObject objectInPool, int countElementsWillBeInPool = 1, params Type[] typesComponent)
    {
        CanAddInPool(keyObjectInPool, objectInPool, countElementsWillBeInPool, typesComponent);
    }

    private void CanAddInPool(PoolKey keyObjectInPool, GameObject objectInPool, int countElementsWillBeInPool = 1, params Type[] typesComponent)
    {
        if (_poolObject.ContainsKey(keyObjectInPool))
        {
            AddElement(countElementsWillBeInPool, keyObjectInPool, objectInPool, typesComponent);
        }
        else 
        {
            _poolObject.Add(keyObjectInPool, new Queue<ObjectInPool>());
            AddElement(countElementsWillBeInPool, keyObjectInPool, objectInPool, typesComponent); 
        }
    }

    private void AddElement(int countElementsWillBeInPool, PoolKey keyObjectInPool, GameObject objectInPool, params Type[] typesComponent)
    {
        for (var i = 0; i < countElementsWillBeInPool; i++)
        {
            AddObjectInPool(keyObjectInPool, objectInPool, false, typesComponent);
        }
    }

    private GameObject AddObjectInPool(PoolKey keyObjectInPool, GameObject prefabObjectObject, bool isActive, params Type[] typesComponent)
    {
        var objectInPool = CreateNewObjectWithComponent(prefabObjectObject, typesComponent);
        objectInPool.GameObject().SetActive(isActive);
        _poolObject[keyObjectInPool].Enqueue(new ObjectInPool(objectInPool));
        return objectInPool;
    }
        
    private GameObject CreateNewObjectWithComponent(GameObject prefabObjectObject, params Type[] typesComponent)
    {
        var prefab = _container.InstantiatePrefab(prefabObjectObject);
        
        typesComponent
            .ToObservable()
            .Where(type => prefab.GetComponent(type) == null)
            .Subscribe(type => prefab.AddComponent(type));
        
        return prefab;
    }

    public GameObject GetElementInPool(PoolKey keyObjectInPool)
    {
        if (HasFreeElementInPool(out var objectInPool, keyObjectInPool))
        {
            return objectInPool;
        }

        if (AutoExpandPool)
            return _poolObject[keyObjectInPool]
                .Where(objectPool => objectPool.PrefabObject.GameObject().activeInHierarchy)
                .Select(objectPool => AddObjectInPool(keyObjectInPool, objectPool.PrefabObject.GameObject(), false))
                .FirstOrDefault();
        Debug.LogWarning($"parameter {nameof(AutoExpandPool)} false and object dont create automatically create manually");
        return null;
    }

    private bool HasFreeElementInPool(out GameObject objectInPool, PoolKey keyObjectInPool)
    {
        foreach (var objectPool in _poolObject[keyObjectInPool].Where(objectPool => !objectPool.PrefabObject.GameObject().activeInHierarchy))
        {
            objectInPool = objectPool.PrefabObject;
            objectInPool.GameObject().SetActive(true);
            return true;
        }

        objectInPool = null;
        return false;
    }
        
    private class ObjectInPool
    {
        public readonly GameObject PrefabObject;
            
        public ObjectInPool(GameObject prefabObject)
        {
            if (prefabObject == null)
                throw new ArgumentNullException($"One of the arguments construct is null {prefabObject} or {prefabObject}");
                
            PrefabObject = prefabObject;
        }
    }
}
