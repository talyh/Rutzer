using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : Singleton<GameController>
{
    public delegate void SpeedIncreased();
    public static event SpeedIncreased speedIncreased;

    // Define Layers and LayerMasks used throughout the game
    // Additional LayerMasks may be defined in individual scripts if they're used only in that script
    public enum Layers { Ground = 8 };
    internal LayerMask ground;

    // // Define Tags used throughout the game
    public enum Tags { Player, SceneBlockPool, SceneBlock };

    // // Define Controls used throughout the game
    // // public enum Controls { Horizontal, Jump, GrabItem, Crouch, Fly}

    private const float INITIAL_SPEED = 2.5f;
    private const float MAX_SPEED = 10;
    private const int HUD_SPEED_MULTIPLIER = 10;

    private const int POINTS_MULTIPLIER = 10;
    private const int POINTS_FOR_SPEED_INCREASE = 100;

    private float _speed;
    private int _score;
    private float _rawScore;
    private int _highScore;

    private bool _newRecord;

    private bool _gameOver = true;

    private Transform _character;

    private ObjectPool _sceneBlockPuller;
    private const float BLOCK_SCENE_SIZE = 17.6f;

    private void Start()
    {
        // Define the LayerMasks that will be needed throughout the game
        ground = 1 << LayerMask.NameToLayer(Layers.Ground.ToString());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PauseGame();
        }

        ScorePoints();
    }

    public void ScorePoints()
    {
        // if game is actually running, score points based on time the player stays alive
        // points are rounded for better UI display and user interpretation
        if (!_gameOver)
        {
            _rawScore += Time.deltaTime;
            if (_rawScore >= 1)
            {
                _rawScore = 0;
                score += POINTS_MULTIPLIER;

                IncreaseSpeedBasaedOnPoints();
            }
        }
    }

    public void GameOver()
    {
        // if game has ended, check for new highscores to be saved and load the Game Over Scene
        _gameOver = true;

        if (_score > _highScore)
        {
            _highScore = _score;
            Persistency.SaveData(Persistency.DataGroups.Score);
            _newRecord = true;
        }

        StartCoroutine(WaitForPlayerDeathEffects());
    }

    private IEnumerator WaitForPlayerDeathEffects()
    {
        yield return new WaitUntil(() => _character.GetComponent<Runner>().readyToDie);
        SceneController.instance.GameOver();
    }

    public void StartGame()
    {
        ResetGameVariables();
        FindCharacter();
        FindSceneBlockPool();
    }

    private void ResetGameVariables()
    {
        speed = INITIAL_SPEED;
        score = 0;
        _rawScore = 0;
        _gameOver = false;
        _newRecord = false;
    }

    private void FindCharacter()
    {
        GameObject go = GameObject.FindGameObjectWithTag(Tags.Player.ToString());
        if (go)
        {
            _character = go.transform;
        }
    }

    private void FindSceneBlockPool()
    {
        GameObject go = GameObject.FindGameObjectWithTag(Tags.SceneBlockPool.ToString());
        if (go)
        {
            ObjectPool pool = go.GetComponent<ObjectPool>();
            if (pool)
            {
                _sceneBlockPuller = pool;
            }
        }
    }

    public void PauseGame()
    {
        // if not yet puased, pause it
        if (Time.timeScale >= 1)
        {
            Supporting.Log("Pausing");
            CanvasController.instance.EnablePauseCanvas(true);
            Time.timeScale = 0;
        }
        // else, unpause it
        else
        {
            Supporting.Log("Unpausing");
            CanvasController.instance.EnablePauseCanvas(false);
            Time.timeScale = 1;
        }
    }

    public void SpawnNewBlockScene(GameObject previousBlockScene)
    {
        GameObject go = _sceneBlockPuller.GetRandomPooledObject();
        go.transform.position = new Vector3(previousBlockScene.transform.position.x + BLOCK_SCENE_SIZE * 2, 0, 0);
        go.SetActive(true);
        previousBlockScene.SetActive(false);
    }

    private void IncreaseSpeedBasaedOnPoints()
    {
        if (_speed >= MAX_SPEED)
        {
            return;
        }

        if (_score > 0 && _score % POINTS_FOR_SPEED_INCREASE == 0)
        {
            speed++;
            _character.GetComponent<Runner>().IncreaseSpeed();
            speedIncreased();
        }
    }

    public float speed
    {
        get { return _speed; }
        set { _speed = value; CanvasController.instance.ShowSpeed((int)speed * HUD_SPEED_MULTIPLIER); }
    }

    public int score
    {
        get { return _score; }
        set { _score = value; CanvasController.instance.ShowScore(score); }
    }

    public int highScore
    {
        get { return _highScore; }
        set { _highScore = value; CanvasController.instance.ShowHighScore(highScore); }
    }

    public bool newRecord
    {
        get { return _newRecord; }
    }

    public Transform character
    {
        get { return _character; }
    }
}

