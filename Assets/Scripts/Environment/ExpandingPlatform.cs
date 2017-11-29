using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class ExpandingPlatform : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _shrinked;
    [SerializeField]
    private SpriteRenderer _expanded;

    private BoxCollider2D _collider;
    [SerializeField]
    private float _bounciness;

    private bool _activated;

    private void Awake()
    {
        RunInitialChecks();
    }

    private void RunInitialChecks()
    {
        Supporting.CheckRequiredProperty(gameObject, _shrinked, "Shrinked Sprite");
        Supporting.CheckRequiredProperty(gameObject, _expanded, "Expanded Sprite");

        _collider = GetComponent<BoxCollider2D>();
        Supporting.CheckRequiredProperty(gameObject, _collider, "Collider");
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(1))
        {
            if (!_activated)
            {
                Expand();
            }
            else
            {
                Shrink();
            }
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
                if (!_activated)
                {
                    Expand();
                }
                else
                {
                    Shrink();
                }
            }
            // if pinching in, do nothing
        }
#endif
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
        if (_collider)
        {
            float duration = 0.2f;
            DOTween.To(() => _collider.size, x => _collider.size = x, (Vector2)_expanded.sprite.bounds.size, duration);
            _shrinked.DOFade(0, 0);
            _expanded.DOFade(100, duration);

        }

        SoundController.instance.PlaySFX(SoundController.instance.sfxStretchPlatform);
    }

    private void Shrink()
    {
        if (!_activated)
        {
            return;
        }

        _activated = false;

        if (_collider)
        {
            float duration = 1;
            DOTween.To(() => _collider.size, x => _collider.size = x, (Vector2)_shrinked.sprite.bounds.size, duration);
            _shrinked.DOFade(100, duration * 200);
            _expanded.DOFade(0, duration * 0.5f);
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == GameController.Tags.Player.ToString() && _activated)
        {
            Shrink();
            coll.rigidbody.AddForce(Vector2.up * _bounciness, ForceMode2D.Impulse);
        }
    }

    private void OnDisable()
    {
        Shrink();
    }
}
