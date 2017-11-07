using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [Header("Camera Movement")]
    // [SerializeField] private float leftEdge = 1;
    // [SerializeField] private float rightEdge = 100;
    // [SerializeField] private float topEdge = 100;
    // [SerializeField] private float bottomEdge = 0;
    // [SerializeField] private float speed = 10;
    private Transform _target;
    private float _offset;

    // Use this for initialization
    void Start()
    {
        // Find the player
        // _target = GameController.instance.character;
        // GameController.instance.CheckRequiredComponent("Target", _target, objectName: gameObject.name);

        if (_target)
        {
            _offset = _target.position.x - transform.position.x;
        }
    }

    void LateUpdate()
    {
        // if the player is still alive, move the camera to the player's position, clamping it to the level edges
        if (_target)
        {
            // Vector3 newCameraPosition = new Vector3(
            //    Mathf.Clamp(_target.position.x, leftEdge, rightEdge),
            //    Mathf.Clamp(_target.position.y, bottomEdge, topEdge),
            //    transform.position.z);
            // using Lerp we have a smoother transition
            // transform.position = Vector3.Lerp(transform.position, newCameraPosition, speed * Time.deltaTime);
            // Vector3 newCameraPosition = new Vector3(_target.position.x - _offset, transform.position.y, transform.position.z);
            transform.position = new Vector3(_target.position.x - _offset, transform.position.y, transform.position.z);
        }
    }
}
