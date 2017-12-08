using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class CoinPack : DownloadableObject
{
    [SerializeField]
    private Coin _prefab;
    [SerializeField]
    private float _padding;
    private CoinPackData _data;

    private void OnEnable()
    {
        _data = CoinPacksDataManager.GetRandom();

        // Supporting.Log(_data.ToString());
        // Supporting.Log("current children: " + transform.childCount);

        for (int i = 0; i < _data.coins; i++)
        {
            Coin coin = Instantiate(_prefab, transform);
            float newX = transform.position.x + _padding * transform.childCount;
            coin.transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }

        // Supporting.Log("new children: " + transform.childCount);
    }

    private void OnDisable()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
