using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class GameController : Singleton<GameController>
{
    // // Define lists used throughout the game
    // // public enum Powers { noPower = 0, grow, extraLife, fly, fire, invincible }
    // public enum CharacterStates { small = 1, big, racoon }; // starting at 1 to keep it aligned with the animation layer index (where 0 = base layer)

    // // Define Layers and LayerMasks used throughout the game
    // // Additional LayerMasks may be defined in individual scripts if they're used only in that script
    public enum Layers { Ground = 8 };
    internal LayerMask ground;
    // internal LayerMask player;
    // internal LayerMask notPlayer;

    // // Define Tags used throughout the game
    // public enum Tags { BashfullnessRange, CheckPoint, DeathPit, Shell, Block, Item, GroundCheck, HoldPoint, ShotSpawnPoint, PlayerSprite, HitPoint };

    // // Define Controls used throughout the game
    // // public enum Controls { Horizontal, Jump, GrabItem, Crouch, Fly}

    private const float INITIAL_SPEED = 2.5f;
    private float _speed;
    private int _score;
    private float _rawScore;
    private int _highScore;

    private const int POINTS_MULTIPLIER = 10;

    private bool _gameOver = true;

    private void Start()
    {
        Persistency.LoadSavedData(Persistency.DataGroups.Score);

        // Get prefab values for dynamic instation if needed
        // GameObject GameControllerPrefab = AssetDatabase.LoadAssetAtPath("Assets/General/Controllers/GameController.prefab", typeof(GameObject)) as GameObject;
        // if (GameControllerPrefab)
        // {
        //     initialLives = GameControllerPrefab.GetComponent<GameController>().initialLives;
        // }
        // else
        // {
        //     Debug.Log("GameController prefab not found");
        // }

        // Define the LayerMasks that will be needed throughout the game
        ground = 1 << LayerMask.NameToLayer(Layers.Ground.ToString());
        // 1 << LayerMask.NameToLayer(Layers.Hitable.ToString()) |
        //  1 << LayerMask.NameToLayer(Layers.Pipes.ToString());
        // player = 1 << LayerMask.NameToLayer(Layers.Player.ToString());
        // notPlayer = ~(1 << LayerMask.NameToLayer(Layers.Player.ToString()));

        // lives = initialLives;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            speed++;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            score++;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            highScore++;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            GameOver();
        }

        ScorePoints();
    }

    public void ScorePoints()
    {
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
        _gameOver = true;
        if (_score > _highScore)
        {
            _highScore = _score;
        }
        Persistency.SaveData(Persistency.DataGroups.Score);
        SceneController.instance.GameOver();
    }

    public void StartGame()
    {
        ResetGameVariables();
        _gameOver = false;
    }

    public void ResetGameVariables()
    {
        speed = INITIAL_SPEED;
        score = 0;
        _rawScore = 0;
        _gameOver = false;
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

    // public Transform character
    // {
    //     get { return _character; }
    // }
}

