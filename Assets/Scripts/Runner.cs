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
    private Transform _groundCheck;
    [SerializeField]
    private float _jumpForce;

    private bool _grounded;
    private bool _gapAhead;


    private void Awake()
    {
        RunInitialChecks();
        InitializeVariables();
    }

    private void Start()
    {
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        _grounded = Physics2D.OverlapCircle(_groundCheck.position, 0.2f, GameController.instance.ground);

        Supporting.Log("grounded: " + _grounded);

        _gapAhead = !_gapCheck.IsTouchingLayers(GameController.instance.ground);

        if (!_gapAhead)
        {
            Run();
        }
        else
        {
            Jump();
        }
    }

    private void Run()
    {
        // Supporting.Log("Running");

        if (_rb.velocity.x <= 0)
        {
            _rb.AddForce(Vector2.right * GameController.instance.speed, ForceMode2D.Impulse);
        }
        else
        {
            _rb.AddForce(Vector2.right * GameController.instance.speed * Time.deltaTime, ForceMode2D.Force);
        }
    }

    private void Jump()
    {
        Supporting.Log("Jumping");

        if (_grounded)
        {
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
    }

    void RunInitialChecks()
    {
        Supporting.CheckRequiredProperty(gameObject, _rb, "Rigidboy");
        Supporting.CheckRequiredProperty(gameObject, _gapCheck, "Gap Check");
        Supporting.CheckRequiredProperty(gameObject, _jumpLevelCheck, "Jump Level Check");
        Supporting.CheckRequiredProperty(gameObject, _jumpHighCheck, "Jump High Check");
    }

    void InitializeVariables()
    {
        _gapAhead = false;
        _grounded = true;
    }
}
