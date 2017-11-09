using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlatform : MonoBehaviour
{
    private const float DROPPING_GRAVITY = 0.8f;

    [SerializeField]
    private Transform _platform;

    private Rigidbody2D _rb;

    private bool _activated;

    private void Awake()
    {
        RunInitialChecks();
    }

    private void RunInitialChecks()
    {
        Supporting.CheckRequiredProperty(gameObject, _platform, "Platform");

        if (_platform)
        {
            _rb = _platform.GetComponent<Rigidbody2D>();
            Supporting.CheckRequiredProperty(_platform.gameObject, _rb, "Platform Rigidbody");
        }
    }

    public void Drop()
    {
        // only attempt dropping if it's the first time
        if (_activated)
        {
            return;
        }
        _activated = true;

        // TODO - replace with proper physics drop based on joints
        _rb.gravityScale = DROPPING_GRAVITY;

        // TODO - ensure platform doesn't get bumped up if player hits it from underneath
    }
}
