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
    private Collider2D _floorCheck;
    [SerializeField]
    private Collider2D _landingCheck;
    [SerializeField]
    private Transform _groundCheck;
    [SerializeField]
    private float _jumpForce;

    [Header("Animation")]
    [SerializeField]
    private RunnerAnimator _runnerAnimator;

    private bool _grounded;
    private bool _wait;
    private bool _gapAhead;
    private bool _floorAhead;
    private bool _readyToDie;
    private bool _facinngLeft;

    private void Awake()
    {
        RunInitialChecks();
    }

    private void Start()
    {
        _runnerAnimator.ChangeColor(RunnerAnimator.AnimationLayers.Grey);
    }

    private void Update()
    {
        // move the character outside of running game
        if (SceneController.instance.currentSceneType != SceneController.SceneTypes.Level)
        {
            int direction = _facinngLeft ? -1 : 1;
            float nexX = transform.position.x + Time.deltaTime * direction * GameData.Constants.GetConstant<float>(GameData.Constants.constantKeywords.INITIAL_SPEED.ToString());
            transform.position = new Vector3(nexX, transform.position.y, transform.position.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        // flip the character outside of running game
        if (GameController.instance.wallLayer == (GameController.instance.wallLayer | 1 << coll.gameObject.layer))
        {
            Flip();
        }
    }

    private void Flip()
    {
        _facinngLeft = !_facinngLeft;

        Vector3 newRotation = transform.rotation.eulerAngles;
        newRotation.y += 180;
        transform.rotation = Quaternion.Euler(newRotation);
    }

    private void FixedUpdate()
    {
        if (GameController.instance.gameOver)
        {
            return;
        }

        // determine whether the character is standing in an area where it should stop and wait
        _wait = Physics2D.Raycast(_groundCheck.position, Vector2.down, Physics2D.defaultContactOffset, GameController.instance.waitLayer);

        // determine if there's a gap to be jumped, based on contact of the gapAhead collider with elements in the ground layer
        _gapAhead = !_gapCheck.IsTouchingLayers(GameController.instance.floorLayer);

        // determine if there's an appropriate landing spot after a gap
        _floorAhead = _floorCheck.IsTouchingLayers(GameController.instance.floorLayer) && !_landingCheck.IsTouchingLayers(GameController.instance.groundLayer);

        if (_wait && _gapAhead)
        {
            Stop();
            // Supporting.Log("Florr Ahead: " + _floorCheck.IsTouchingLayers(GameController.instance.groundLayer) + " / " + !_landingCheck.IsTouchingLayers(GameController.instance.groundLayer));
        }
        else
        {
            // determine whether the character's feet are touching the floor or not, based on contact of its groundCheck with elements in the ground layer
            _grounded = Physics2D.Raycast(_groundCheck.position, Vector2.down, Physics2D.defaultContactOffset, GameController.instance.groundLayer);
        }

        if (_wait || _grounded)
        {
            // if no gapahead, run
            // if there's a gap ahead, and a landing spot ahead of it, jump
            // else, take a leap of faith
            if (!_gapAhead)
            {
                Run();
            }
            else if (_floorAhead)
            {
                Jump();
            }
        }
    }

    private void Run()
    {
        if (_rb.velocity.x <= 0.1f)
        {
            if (GameController.instance.speed == GameData.Constants.GetConstant<float>(GameData.Constants.constantKeywords.INITIAL_SPEED.ToString()))
            {
                // Supporting.Log("Running with impulse. RB Velocity: " + _rb.velocity.x);
                _rb.AddForce(Vector2.right * GameController.instance.speed, ForceMode2D.Impulse);

                // Supporting.Log("new velocity: " + _rb.velocity.x);
            }
            else
            {
                // Supporting.Log("Manually nudging the character from " + transform.position + " to " + (transform.position + new Vector3(0.1f, 0.1f, 0)));
                transform.position = transform.position + new Vector3(0.1f, 0, 0);
                _rb.AddForce(Vector2.right * GameController.instance.speed, ForceMode2D.Impulse);
            }
        }
    }

    private void Jump()
    {
        // Supporting.Log("Jumping");

        float verticalImpulse = _jumpForce / GameController.instance.speed;

        if (_rb.velocity.x > 0)
        // add vertical impulse to the character, based on its jumpForce
        {
            _rb.AddForce(Vector2.up * verticalImpulse, ForceMode2D.Impulse);
        }
        else
        {
            _rb.AddForce(new Vector2(verticalImpulse, GameController.instance.speed), ForceMode2D.Impulse);
        }

        // play the jumping sfx
        SoundController.instance.PlaySFX(SoundController.instance.sfxJump);
    }

    private void Stop()
    {
        _rb.velocity = Vector2.zero;
        _rb.Sleep();
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
        _rb.gravityScale *= (1 + GameController.instance.speed / 100);

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
        yield return new WaitForSeconds(1.5f);
        _readyToDie = true;
    }

    private void RunInitialChecks()
    {
        Supporting.CheckRequiredProperty(gameObject, _rb, "Rigidboy");
        Supporting.CheckRequiredProperty(gameObject, _gapCheck, "Gap Check");
        Supporting.CheckRequiredProperty(gameObject, _floorCheck, "Jump Level Check");
        Supporting.CheckRequiredProperty(gameObject, _landingCheck, "Jump High Check");
        Supporting.CheckRequiredProperty(gameObject, _runnerAnimator, "Runner Animator");
    }

    public bool readyToDie
    {
        get { return _readyToDie; }
    }
}