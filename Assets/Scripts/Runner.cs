using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Runner : MonoBehaviour
{

    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private Collider2D _gapCheck;
    [SerializeField]
    private Collider2D _jumpLevelCheck;
    [SerializeField]
    private Collider2D _jumpHighCheck;
    [SerializeField]
    private float _jumpForce;

    private bool _gapAhead;


    private void Awake()
    {
        RunInitialChecks();
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        // Supporting.Log("Running at: " + _rb.velocity.x);

        _gapAhead = !_gapCheck.IsTouchingLayers(GameController.instance.ground);

        if (!_gapAhead)
        {
            _rb.velocity = Vector2.right * GameController.instance.speed * Time.deltaTime;
        }
        else
        {
            Jump();
        }
    }

    private void Jump()
    {
        _rb.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
    }

    void RunInitialChecks()
    {
        Supporting.CheckRequiredProperty(gameObject, _rb, "Rigidboy");
        Supporting.CheckRequiredProperty(gameObject, _gapCheck, "Gap Check");
        Supporting.CheckRequiredProperty(gameObject, _jumpLevelCheck, "Jump Level Check");
        Supporting.CheckRequiredProperty(gameObject, _jumpHighCheck, "Jump High Check");
    }
}
