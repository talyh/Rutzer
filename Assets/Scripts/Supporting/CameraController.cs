using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float INITIAL_X_FOR_4_3 = 5.4f;
    private const float INITIAL_X_FOR_16_10 = 6.8f;
    private const float INITIAL_X_FOR_16_9 = 7.6f;

    private Transform _target;
    private float _offset;

    private float _resolution;
    public enum Resolution { _16_9, _16_10, _4_3 }
    public Resolution currentResolution
    {
        get
        {
            if (_resolution <= 1.4f)
            {
                return Resolution._4_3;
            }
            else if (_resolution <= 1.7f)
            {
                return Resolution._16_10;
            }
            else
            {
                return Resolution._16_9;
            }
        }
    }


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
        switch (currentResolution)
        {
            case Resolution._4_3:
                {
                    transform.position = new Vector3(INITIAL_X_FOR_4_3, transform.position.y, transform.position.z);
                    break;
                }
            case Resolution._16_10:
                {
                    transform.position = new Vector3(INITIAL_X_FOR_16_10, transform.position.y, transform.position.z);
                    break;
                }
            case Resolution._16_9:
            default:
                {
                    transform.position = new Vector3(INITIAL_X_FOR_16_9, transform.position.y, transform.position.z);
                    break;
                }
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
