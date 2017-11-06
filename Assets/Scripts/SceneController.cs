using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : Singleton<SceneController>
{
    private int _currentScene;

    private int lastScene = 0;

    public enum SceneTypes { Start = 0, Instructions, Options, Credits, Level, GameOver }

    private SceneTypes _currentSceneType;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnRuntimeMethodLoad()
    {
        // Add the delegate to be called when the scene is loaded, between Awake and Start.
        SceneManager.sceneLoaded += SceneLoaded;
    }

    static void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        //Debug.Log(System.String.Format("Scene{0} has been loaded ({1})", scene.name, loadSceneMode.ToString()));
        instance.SetCurrentSceneIndex();
        instance.SetCurrentSceneType();
        CanvasController.instance.EnableSceneCanvas();
        // SoundController.instance.EnableSceneMusic();
    }

    void Start()
    {
        lastScene = SceneManager.sceneCountInBuildSettings - 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            LoadPrevious();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            LoadNext();
        }
    }

    public void LoadPrevious()
    {
        SetCurrentSceneIndex();
        if (_currentScene > 0)
        {
            SceneManager.LoadScene(_currentScene - 1);
        }
        else
        {
            Debug.Log("Already on first scene");
        }
    }

    public void LoadNext()
    {
        SetCurrentSceneIndex();
        if (_currentScene < lastScene)
        {
            SceneManager.LoadScene(_currentScene + 1);
        }
        else
        {
            Debug.Log("Already on last scene");
        }
    }

    public void Reload()
    {
        SetCurrentSceneIndex();
        SceneManager.LoadScene(_currentScene);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1); // assume 0 is the welcome screen
    }

    public void RestartGame()
    {
        // GameController.instance.ResetGameVariables();
        SceneManager.LoadScene(1); // assume 0 is the welcome screen
    }

    public void GameOver()
    {
        SceneManager.LoadScene(lastScene); //assume GameOver is the last scene
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
				Application.Quit();
#endif
    }

    public void ShowInstructionsScene()
    {
        Debug.Log("Instructions");
    }

    public void ShowOptionsScene()
    {
        Debug.Log("Options");
    }

    public void ShowCreditsScene()
    {
        Debug.Log("Credits");
    }

    public void SetCurrentSceneIndex()
    {
        _currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    public void SetCurrentSceneType()
    {
        if (_currentScene == 0)
        {
            _currentSceneType = SceneTypes.Start;
        }
        else if (_currentScene == lastScene)
        {
            _currentSceneType = SceneTypes.GameOver;
        }
        else
        {
            _currentSceneType = SceneTypes.Level;
        }
    }

    new void OnDestroy()
    {
        applicationIsQuitting = true;

        // Remove the delegate when the object is destroyed
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    public SceneTypes currentSceneType
    {
        set { SetCurrentSceneType(); }
        get { return _currentSceneType; }
    }
}
