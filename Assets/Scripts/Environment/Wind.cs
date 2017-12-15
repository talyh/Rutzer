using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public delegate void WindBlowing(bool active, float windForceX, float windForceY);
    public static event WindBlowing windBlowing;

    public float windForceX
    {
        get { return GameController.instance.speed * -0.10f; }
    }
    public const float windForceY = 0;


    private void OnEnable()
    {
        windBlowing(true, windForceX, windForceY);
    }

    private void OnDisable()
    {
        windBlowing(false, 0, 0);
    }
}
