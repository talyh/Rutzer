using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevModeControls : MonoBehaviour
{
    void Awake()
    {
        if (!Debug.isDebugBuild)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Debug.Log(string.Format("speed: {0}", GameData.Constants.GetConstant<float>("speed")));
    }

}
