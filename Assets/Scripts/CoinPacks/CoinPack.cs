using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class CoinPack : MonoBehaviour
{
    private CoinPackData _data;

    private void OnEnable()
    {
        _data = CoinPacksDataManager.GetRandom();
        Debug.Log(_data);
    }
}
