using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class ExpandingPlatform : MonoBehaviour
{
    // REMINDER -----------
    // on pinch, the center object is replaced by the expanded image, and
    // the box collider is resized to fit it, automatically pushing the corners
    // the material on the collider should have bounciness
    // on collision of the player with the center object should replace it with the squished image and resize it to small

    [SerializeField]
    private Sprite _shrinked;
    [SerializeField]
    private Sprite _expanded;
    private SpriteRenderer _renderer;
    private BoxCollider2D _collider;

    private bool _activated;
    private void Awake()
    {
        RunInitialChecks();
    }

    private void RunInitialChecks()
    {
        Supporting.CheckRequiredProperty(gameObject, _shrinked, "Shrinked Sprite");
        Supporting.CheckRequiredProperty(gameObject, _expanded, "Expanded Sprite");

        _renderer = GetComponent<SpriteRenderer>();
        Supporting.CheckRequiredProperty(gameObject, _renderer, "Sprite Renderer");

        _collider = GetComponent<BoxCollider2D>();
        Supporting.CheckRequiredProperty(gameObject, _collider, "Collider");
    }

    private void Update()
    {
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

        // adjust the collider side, so the spring joint this object is attached to takes care of pushing the corners to the side
        if (_renderer && _collider)
        {
            _renderer.sprite = _expanded;
            _collider.size = new Vector2(2.4f, _collider.size.y);
            // _collider.size = _renderer.sprite.bounds.size;
            // _collider.size = new Vector2(3.2f, 0.8f);
            // _collider.size = Vector2.Lerp(_collider.size, _renderer.sprite.bounds.size, Time.deltaTime);
        }

        SoundController.instance.PlaySFX(SoundController.instance.sfxStretchPlatform);
    }

    // private void Shrink()
    // {
    //     // TODO - replace with proper expansion and shrinking based on Physics Joint
    //     _shrinked.gameObject.SetActive(true);
    //     _expanded.gameObject.SetActive(false);
    // }
}
