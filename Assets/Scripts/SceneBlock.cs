using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBlock : MonoBehaviour
{
    [SerializeField]
    private VerticalPlatform _verticalPlatform;

    [SerializeField]
    private ExpandingPlatform _expandingPlatform;

    private bool _visible;
    private float _elapsedTimeBetweenTouches;
    private bool _listenForTouches = true;

    private void Update()
    {
        // if not listening for touches, count until the point when should start listening again
        if (!_listenForTouches)
        {
            _elapsedTimeBetweenTouches++;

            if (_elapsedTimeBetweenTouches <= 5)
            {
                return;
            }
            else
            {
                _elapsedTimeBetweenTouches = 0;
                _listenForTouches = true;
            }
        }

        // if tapping, drop the vertical platform, if it's present
        // stop listening for touches for a little while to avoid interference with the next action
        if (Input.touchCount == 1)
        {
            if (_verticalPlatform)
            {
                _verticalPlatform.Drop();
            }
            _listenForTouches = false;
        }

        // if pinching, expand the expanding platform, if it's present
        // stop listening for touches for a little while to avoid interference with the next action
        if (Input.touchCount == 2)
        {
            if (_expandingPlatform)
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
                    _expandingPlatform.Expand();

                }
                // if pinching in, do nothing
            }
            _listenForTouches = false;
        }
    }
}
