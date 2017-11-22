using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SliderJoint2D))]
public class VerticalPlatform : MonoBehaviour
{
    [SerializeField]
    private GameObject _topEdge;
    [SerializeField]
    private GameObject _bottomEdge;
    private SliderJoint2D _slider;

    private float _originalY;

    private bool _activated;

    private void Awake()
    {
        RunInitialChecks();
        _originalY = transform.position.y;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            Activate();
        }
#endif

#if UNITY_ANDROID || UNITY_IOS
        // if tapping, drop the vertical platform, if it's present
        // stop listening for touches for a little while to avoid interference with the next action
        if (Input.touchCount == 1)
        {
            Activate();
        }
#endif
    }

    private void RunInitialChecks()
    {
        _slider = GetComponent<SliderJoint2D>();
        Supporting.CheckRequiredProperty(gameObject, _slider, "Slider Joint");
    }

    public void Activate()
    {
        // only attempt dropping if it's the first time
        if (_activated)
        {
            return;
        }
        _activated = true;

        _slider.useMotor = true;
        SoundController.instance.PlaySFX(SoundController.instance.sfxMovePlatform);
    }

    private void Deactivate()
    {
        _slider.useMotor = false;
        _activated = false;
    }

    private void ChangeDirection()
    {
        // change the direction of the motor
        JointMotor2D newMotor = _slider.motor;
        newMotor.motorSpeed = -newMotor.motorSpeed;
        _slider.motor = newMotor;
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject == _topEdge || coll.gameObject == _bottomEdge)
        {
            // Supporting.Log("Vertical platform hit an edge");
            ChangeDirection();
        }
    }

    private void OnDisable()
    {
        // Supporting.Log("Vertical Platform disabled");
        Reset();
    }

    private void Reset()
    {
        Deactivate();
        transform.position = new Vector3(transform.position.x, _originalY, transform.position.z);
    }
}
