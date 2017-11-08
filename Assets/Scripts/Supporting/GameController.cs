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
    // public enum Layers { Ground = 8, Grabable, PowerUp, Hitable, Edges, Pipes, Player };
    // internal LayerMask ground;
    // internal LayerMask player;
    // internal LayerMask notPlayer;

    // // Define Tags used throughout the game
    // public enum Tags { BashfullnessRange, CheckPoint, DeathPit, Shell, Block, Item, GroundCheck, HoldPoint, ShotSpawnPoint, PlayerSprite, HitPoint };

    // // Define Controls used throughout the game
    // // public enum Controls { Horizontal, Jump, GrabItem, Crouch, Fly}

    // [Header("General Game Settings")]
    // // [SerializeField]
    // // public const float hurryUpMusicRate = 0.25f;

    // // Identify character
    // [Header("Character")]
    // // [SerializeField]
    // // [Tooltip("How many lives the character starts with")]
    // // private int _initialLives = 0;
    // // private int _lives = 0;
    // // private int _score = 0;
    // // private int _coins = 0;
    // [SerializeField]
    // private Transform _character;

    void Start()
    {
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
        // ground = 1 << LayerMask.NameToLayer(Layers.Ground.ToString()) |
        //             1 << LayerMask.NameToLayer(Layers.Hitable.ToString()) |
        //              1 << LayerMask.NameToLayer(Layers.Pipes.ToString());
        // player = 1 << LayerMask.NameToLayer(Layers.Player.ToString());
        // notPlayer = ~(1 << LayerMask.NameToLayer(Layers.Player.ToString()));

        // lives = initialLives;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneController.instance.GameOver();
        }
    }

    // Game helper methods
    // public void CheckRequiredFloat(string name, float f, float def = 0.2f, string objectName = "unknown")
    // {
    //     if (f <= 0f)
    //     {
    //         f = def;
    //         Debug.LogWarning(objectName + " >> " + name + " was missing. Automatically adjusted to " + f);
    //     }
    // }

    // public void CheckRequiredComponent(string name, Component c, string objectName = "unknown")
    // {
    //     if (!c)
    //     {
    //         Debug.LogError(objectName + " >> " + name + " is required");
    //     }
    // }

    // public void CheckRequiredChild(string name, Transform t, string objectName = "unknown")
    // {
    //     if (!t)
    //     {
    //         Debug.LogError(objectName + " >> " + name + " is required");
    //     }
    // }

    // public void CheckRequiredVector2(string name, Vector2 v, float x = 0.1f, float y = 0.1f, string objectName = "unknown")
    // {
    //     if (v.x <= 0 && v.y <= 0)
    //     {
    //         v.x = x;
    //         v.y = x;
    //         Debug.LogWarning(objectName + " >> " + name + " was missing. Automatically adjusted to (" + v.x + ", " + v.y + ")");
    //     }
    // }

    // public void CheckRequiredVector3(string name, Vector3 v, float x = 0.1f, float y = 0.1f, float z = 0.1f, string objectName = "unknown")
    // {
    //     if (v.x <= 0 && v.y <= 0)
    //     {
    //         v.x = x;
    //         v.y = y;
    //         v.z = z;
    //         Debug.LogWarning(objectName + " >> " + name + " was missing. Automatically adjusted to (" + v.x + ", " + v.y + ", " + v.z + ")");
    //     }
    // }

    // Run Game methods
    // public void SpawnCharacter()
    // {
    //     GameObject[] checkPoints = GameObject.FindGameObjectsWithTag(Tags.CheckPoint.ToString());
    //     if (checkPoints.Length <= 0)
    //     {
    //         Debug.Log("Cannot find the last checkpoint to spawn character");
    //     }
    //     else
    //     {
    //         GameObject spawnPosition = checkPoints[0];
    //         _character = _characterPrefab.transform;
    //         Instantiate(character, spawnPosition.transform.position, spawnPosition.transform.rotation);
    //     }
    // }

    // public void ScorePoints(int points)
    // {
    //     score += points;
    // }

    // public void ShowScore()
    // {
    //     if (CanvasController.instance.txtScore)
    //     {
    //         CanvasController.instance.txtScore.text = score.ToString();
    //     }
    // }

    // public void ShowLives()
    // {
    //     if (CanvasController.instance.txtLives)
    //     {
    //         CanvasController.instance.txtLives.text = lives.ToString();
    //     }
    // }

    // public void ShowCoins()
    // {
    //     if (CanvasController.instance.txtCoins)
    //     {
    //         CanvasController.instance.txtCoins.text = coins.ToString();
    //     }
    // }

    // public void StartLevel()
    // {
    //     // Find the character in scene (assume only one)
    //     SpawnCharacter();
    //     _character = FindObjectOfType<Character>().transform;
    //     CheckRequiredComponent("Character", character, objectName:gameObject.name);
    //     ShowScore();
    //     ShowLives();
    //     ShowCoins();
    // }

    // public void ResetGameVariables()
    // {
    //     lives = initialLives;
    //     score = 0;
    //     coins = 0;
    // }

    // public int score
    // {
    //     get { return _score; }
    //     set { _score = value; /*ShowScore();*/ }
    // }

    // public int initialLives
    // {
    //     get { return _initialLives; }
    //     set { _initialLives = value; }
    // }

    // public int lives
    // {
    //     get { return _lives; }
    //     set
    //     { 
    //         if (value > 0)
    //         {
    //             _lives = value; 
    //         }
    //         else
    //         {
    //             _lives = 0;
    //         }
    //         // ShowLives(); 
    //     }
    // }

    // public int coins
    // {
    //     get { return _coins; }
    //     set { _coins = value; /*ShowCoins();*/ }
    // }


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


    // public Transform character
    // {
    //     get { return _character; }
    // }
}
