using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class GameController : Singleton<GameController>
{
    // Define Layers and LayerMasks used throughout the game
    // Additional LayerMasks may be defined in individual scripts if they're used only in that script
    public enum Layers { Ground = 8 };
    internal LayerMask ground;

    // // Define Tags used throughout the game
    public enum Tags { Player, SceneBlockPool, SceneBlock };

    // // Define Controls used throughout the game
    // // public enum Controls { Horizontal, Jump, GrabItem, Crouch, Fly}

    private const float INITIAL_SPEED = 1.5f;
    private float _speed;
    private int _score;
    private float _rawScore;
    private int _highScore;

    private const int POINTS_MULTIPLIER = 10;

    private bool _gameOver = true;

    private Transform _character;

    private ObjectPool _sceneBlockPuller;

    private void Start()
    {
        Persistency.LoadSavedData(Persistency.DataGroups.Score);

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

        // if (Time.frameCount % 100 == 0)
        // {
        //     GameObject go = _sceneBlockPuller.GetPooledObject();
        //     go.SetActive(true);
        //     go.transform.position = new Vector3(character.transform.position.x + 5, 0, 0);
        // }
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
        }
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

    public float speed
    {
        get { return _speed; }
        set { _speed = value; CanvasController.instance.ShowSpeed((int)speed); }
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

    public Transform character
    {
        get { return _character; }
    }
}

