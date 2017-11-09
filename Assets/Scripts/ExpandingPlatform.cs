using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandingPlatform : MonoBehaviour
{
    [SerializeField]
    private Transform _shrinked;
    [SerializeField]
    private Transform _expanded;

    private bool _activated; // TODO - temp, until proper Physics is in place
    private float _timeExpanded; // TODO - temp, until proper Physics is in place

    private void Awake()
    {
        RunInitialChecks();
    }

    private void RunInitialChecks()
    {
        Supporting.CheckRequiredProperty(gameObject, _shrinked, "Shrinked");
        Supporting.CheckRequiredProperty(gameObject, _expanded, "Expanded");
    }

    private void Update()
    {
        if (_activated)
        {
            _timeExpanded++;

            if (_timeExpanded > 10)
            {
                Shrink();
            }
        }
    }

    public void Expand()
    {
        Supporting.Log("Expanding platform");

        // only attempt expanding if it's the first time
        if (_activated)
        {
            return;
        }
        _activated = true;

        // TODO - replace with proper expansion and shrinking based on Physics Joint
        _shrinked.gameObject.SetActive(false);
        _expanded.gameObject.SetActive(true);
    }

    private void Shrink()
    {
        // TODO - replace with proper expansion and shrinking based on Physics Joint
        _shrinked.gameObject.SetActive(true);
        _expanded.gameObject.SetActive(false);
    }
}
