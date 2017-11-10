using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private List<GameObject> _pooledObjects;
    [SerializeField]
    private GameObject[] _objectsInSceneToAddToPool;
    [SerializeField]
    private GameObject[] _objectsToPool;
    [SerializeField]
    private int _amountToPoolInitially;
    [SerializeField]
    private bool _spawnAdditionalIfNeeded;


    private void Start()
    {
        _pooledObjects = new List<GameObject>();

        // if there's any scene object to add, include those
        for (int i = 0; i < _objectsInSceneToAddToPool.Length; i++)
        {
            _pooledObjects.Add(_objectsInSceneToAddToPool[i]);
        }

        // spawn initial objects
        for (int i = 0; i < _amountToPoolInitially; i++)
        {
            SpawnObject();
        }
    }

    public GameObject GetPooledObject()
    {
        // loop through the pool until we find an available object
        // NOTE: it's up to the caller to reactivate and reposition the object, as needed
        for (int i = 0; i < _pooledObjects.Count; i++)
        {
            if (!_pooledObjects[i].activeInHierarchy)
            {
                return _pooledObjects[i];
            }
        }

        // if we didn't find any and new ones should be instantiated during game, create a new one
        if (_spawnAdditionalIfNeeded)
        {
            return SpawnObject();
        }
        return null;
    }

    private GameObject SpawnObject()
    {
        if (_objectsToPool.Length <= 0)
        {
            Supporting.Log("No objects defined to create pool", 1);
            return null;
        }

        // use first object in the array by default, but, if there's more than one option, randomly choose one
        int selectedObject = 0;
        if (_objectsToPool.Length > 1)
        {
            selectedObject = Random.Range(0, _objectsToPool.Length);
        }

        // instantiate a new inactive object, setting it to inactive and adding it to the pool for later usage
        GameObject obj = (GameObject)Instantiate(_objectsToPool[selectedObject]);
        obj.SetActive(false);
        _pooledObjects.Add(obj);
        return obj;
    }
}
