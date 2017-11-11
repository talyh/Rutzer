using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float INITIAL_X_FOR_1_3 = 5.4f;
    private const float INITIAL_X_FOR_1_6 = 6.8f;
    private const float INITIAL_X_FOR_1_7 = 7.6f;

    private Transform _target;
    private float _offset;

    private float _resolution;


    private void Awake()
    {
        DetermineResolution();
        AdjustInitialPositionBasedOnResolution();
    }

    private void Start()
    {
        RunInitialChecks();
    }

    void LateUpdate()
    {
        // follow the player, keeping a stable distance
        if (_target)
        {
            transform.position = new Vector3(_target.position.x - _offset, transform.position.y, transform.position.z);
        }
    }

    private void DetermineResolution()
    {
        Camera cam = GetComponent<Camera>();
        float width = cam.scaledPixelWidth;
        float height = cam.scaledPixelHeight;
        _resolution = width / height;
    }

    private void AdjustInitialPositionBasedOnResolution()
    {
        if (_resolution <= 1.4f)
        {
            transform.position = new Vector3(INITIAL_X_FOR_1_3, transform.position.y, transform.position.z);
        }
        else if (_resolution >= 1.5f && _resolution <= 1.7f)
        {
            transform.position = new Vector3(INITIAL_X_FOR_1_6, transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(INITIAL_X_FOR_1_7, transform.position.y, transform.position.z);
        }
    }

    private void RunInitialChecks()
    {
        // Find the player
        _target = GameController.instance.character;

        if (Supporting.CheckRequiredProperty(gameObject, _target, "Target"))
        {
            _offset = _target.position.x - transform.position.x;
        }
    }
}
