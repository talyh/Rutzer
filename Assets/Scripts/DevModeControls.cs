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
}
