using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownloadableObjectPool : ObjectPool
{
    [SerializeField]
    private CoinPack _shell;

    protected override void CreatePool()
    {
        _pooledObjects = new List<GameObject>();

        for (int i = 0; i < _amountToPoolInitially; i++)
        {
            SpawnObject();
        }
    }

    protected override GameObject SpawnObject(bool initial = false, int index = 0)
    {
        // instantiate a new inactive object, adding it to the pool for later usage
        CoinPack cp = Instantiate(_shell);
        cp.gameObject.SetActive(false);
        _pooledObjects.Add(cp.gameObject);
        return cp.gameObject;
    }
}
