using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(RunnerAnimator))]
public class Runner : MonoBehaviour
{
    [Header("Movement")]
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

    [Header("Animation")]
    [SerializeField]
    private RunnerAnimator _runnerAnimator;

    private bool _grounded;
    private bool _gapAhead;
    private bool _floorAhead;

    private bool _inSlope;

    private bool _readyToDie;


    private void Awake()
    {
        RunInitialChecks();
    }

    private void Start()
    {
        _runnerAnimator.ChangeColor(RunnerAnimator.AnimationLayers.Grey);
    }

    private void FixedUpdate()
    {
        if (GameController.instance.gameOver)
        {
            return;
        }

        // determine whether the character's feet are touching the floor or not, based on contact of its groundCheck with elements in the ground layer
        _grounded = Physics2D.Raycast(_groundCheck.position, Vector2.down, Physics2D.defaultContactOffset, GameController.instance.ground);

        // determine if there's a gap to be jumped, based on contact of the gapAhead collider with elements in the ground layer
        _gapAhead = !_gapCheck.IsTouchingLayers(GameController.instance.ground);

        // determine if there's an appropriate landing spot after a gap
        _floorAhead = _jumpLevelCheck.IsTouchingLayers(GameController.instance.ground);

        if (_grounded)
        {
            if (!_gapAhead)
            {
                Run();
            }
            else
            {
                if (_floorAhead)
                {
                    Jump();
                }
            }
        }
    }

    private void Run()
    {
        if (_rb.velocity.x <= 0.1f)
        {
            if (GameController.instance.speed == GameData.Constants.GetConstant<float>(GameData.Constants.constantKeywords.INITIAL_SPEED.ToString()))
            {
                Supporting.Log("Running with impulse. RB Velocity: " + _rb.velocity.x);

                _rb.AddForce(Vector2.right * GameController.instance.speed, ForceMode2D.Impulse);

                Supporting.Log("new velocity: " + _rb.velocity.x);
            }
            else
            {
                Supporting.Log("Manually nudging the character from " + transform.position + " to " + (transform.position + new Vector3(0.1f, 0.1f, 0)));
                transform.position = transform.position + new Vector3(0.1f, 0, 0);
                _rb.AddForce(Vector2.right * GameController.instance.speed, ForceMode2D.Impulse);
            }
        }
    }

    private void Jump()
    {
        // Supporting.Log("Jumping");

        // add vertical impulse to the character, based on its jumpForce
        _rb.AddForce(Vector2.up * _jumpForce / GameController.instance.speed, ForceMode2D.Impulse);

        // play the jumping sfx
        SoundController.instance.PlaySFX(SoundController.instance.sfxJump);
    }

    public void IncreaseSpeed()
    {
        if (GameController.instance.gameOver)
        {
            return;
        }

        // Supporting.Log("Increasing speed");

        // adjust character's speed, based on new value set in GameController
        _rb.AddForce(Vector2.right, ForceMode2D.Impulse);

        // play speed increase animation and then, change the color, based on the current speed
        StartCoroutine(_runnerAnimator.FlashRainbow(30));
        _runnerAnimator.ChangeColor((RunnerAnimator.AnimationLayers)(GameController.instance.speed -
            GameData.Constants.GetConstant<float>(GameData.Constants.constantKeywords.INITIAL_SPEED.ToString())));
        _runnerAnimator.AdjustSpeed();

        // play the speed increase sfx
        SoundController.instance.PlaySFX(SoundController.instance.sfxIncreaseSpeed);

        // Supporting.Log("RB Velocity: " + _rb.velocity.x);
    }

    public void Die()
    {
        if (GameController.instance.gameOver)
        {
            return;
        }

        _runnerAnimator.Die();
        _rb.bodyType = RigidbodyType2D.Static;
        SoundController.instance.PlaySFX(SoundController.instance.sfxDie);
        StartCoroutine(CheckIfReadyToEndGame());
        GameController.instance.GameOver();
    }

    private IEnumerator CheckIfReadyToEndGame()
    {
        yield return new WaitForSeconds(SoundController.instance.sfxDie.length);
        _readyToDie = true;
    }

    private void RunInitialChecks()
    {
        Supporting.CheckRequiredProperty(gameObject, _rb, "Rigidboy");
        Supporting.CheckRequiredProperty(gameObject, _gapCheck, "Gap Check");
        Supporting.CheckRequiredProperty(gameObject, _jumpLevelCheck, "Jump Level Check");
        Supporting.CheckRequiredProperty(gameObject, _jumpHighCheck, "Jump High Check");
        Supporting.CheckRequiredProperty(gameObject, _runnerAnimator, "Runner Animator");
    }

    public bool readyToDie
    {
        get { return _readyToDie; }
    }
}