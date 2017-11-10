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
        CreatePool();
    }

    public GameObject GetFirstAvailablePooledObject()
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

    public GameObject GetRandomPooledObject()
    {
        int selected = Random.Range(0, _pooledObjects.Count);

        // Supporting.Log("SceneBlock selected; " + selected);

        // check if selected is available, and return it if its
        if (!_pooledObjects[selected].activeInHierarchy)
        {
            // Supporting.Log("Scene Block is available");
            return _pooledObjects[selected];
        }
        else
        {
            // Supporting.Log("Scene Block is not available");
            return GetRandomPooledObject();
        }
    }

    private void CreatePool()
    {
        _pooledObjects = new List<GameObject>();

        // spawn each of the objects listed to pool
        for (int i = 0; i < _objectsToPool.Length; i++)
        {
            SpawnObject(true, i);
        }

        // if needed, spawn extras
        if (_amountToPoolInitially > 0)
        {
            for (int i = 0; i < _amountToPoolInitially; i++)
            {
                SpawnObject();
            }
        }

        // if there's any scene object to add, include those
        for (int i = 0; i < _objectsInSceneToAddToPool.Length; i++)
        {
            _pooledObjects.Add(_objectsInSceneToAddToPool[i]);
        }
    }

    private GameObject SpawnObject(bool initial = false, int index = 0)
    {
        if (_objectsToPool.Length <= 0)
        {
            Supporting.Log("No objects defined to create pool", 1);
            return null;
        }

        // if spawning initial objects, spawn the selected one
        int selectedObject = index;

        // else, spawn one at random, assuming there's more than one option
        if (!initial)
        {
            if (_objectsToPool.Length > 1)
            {
                selectedObject = Random.Range(0, _objectsToPool.Length);
            }
        }

        // instantiate a new inactive object, adding it to the pool for later usage
        GameObject obj = (GameObject)Instantiate(_objectsToPool[selectedObject]);
        obj.SetActive(false);
        _pooledObjects.Add(obj);
        return obj;
    }
}
