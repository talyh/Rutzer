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

            if (_timeExpanded > 50)
            {
                Shrink();
            }
        }
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(1))
        {
            Expand();
        }
#endif

#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount == 2)
        {

            // identify each of the touches
            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);

            // check the touch starting position, for each touch
            Vector3 firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
            Vector3 secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

            // check the distance between the starting positions and ending positions, to help
            // determine the direction of the pinch (in or out)
            float previousTouchDeltaMagnitude = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
            float touchDeltaMagnitude = (firstTouch.position - secondTouch.position).magnitude;

            // if pinching out, expand the platform
            if (previousTouchDeltaMagnitude < touchDeltaMagnitude)
            {
                Expand();

            }
            // if pinching in, do nothing
#endif
        }
    }

    public void Expand()
    {
        // only attempt expanding if it's the first time
        if (_activated)
        {
            return;
        }
        _activated = true;

        // TODO - replace with proper expansion and shrinking based on Physics Joint
        _shrinked.gameObject.SetActive(false);
        _expanded.gameObject.SetActive(true);

        SoundController.instance.PlaySFX(SoundController.instance.sfxStretchPlatform);
    }

    private void Shrink()
    {
        // TODO - replace with proper expansion and shrinking based on Physics Joint
        _shrinked.gameObject.SetActive(true);
        _expanded.gameObject.SetActive(false);
    }
}
