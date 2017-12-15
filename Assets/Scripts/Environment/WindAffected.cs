using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WindAffected : MonoBehaviour
{
    private Rigidbody2D _rb;

    private void OnEnable()
    {
        Wind.windBlowing += BlowWind;
    }

    private void Awake()
    {
        RunInitialChecks();
    }

    private void BlowWind(bool active, float windForceX, float windForceY)
    {
        if (active)
        {
            _rb.AddForce(new Vector2(windForceX, windForceY));
        }
    }

    private void RunInitialChecks()
    {
        _rb = GetComponent<Rigidbody2D>();

        Supporting.CheckRequiredProperty(gameObject, _rb, "Rigidbody");
    }

    private void OnDisable()
    {
        Wind.windBlowing -= BlowWind;
    }
}
