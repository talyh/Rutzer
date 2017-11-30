using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class CoinPack : MonoBehaviour
{

    private CoinPackData _data;

    private void Start()
    {
        if (_data == null)
        {
            _data = CoinPacksDataManager.Get("pack1");
        }
        Debug.Log(_data);
    }
}
