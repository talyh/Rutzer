using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : Singleton<SceneController>
{
    [SerializeField]
    private string _startSceneName = "Welcome";
    [SerializeField]
    private string _instructionsSceneName = "Instructions";
    [SerializeField]
    private string _optionsSceneName = "Options";
    [SerializeField]
    private string _creditsSceneName = "Credits";
    [SerializeField]
    private string _gameOverSceneName = "Game Over";
    [SerializeField]
    private string _firstLevelName = "InfiniteRunner";

    private int _currentScene;
    private string _currentSceneName;

    private int _lastScene = 0;

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
        Debug.Log("loading scene");
    }

    protected override void AdditionalAwakeTasks()
    {
        _lastScene = SceneManager.sceneCountInBuildSettings - 1;
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
        if (_currentScene < _lastScene)
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
        SceneManager.LoadScene(_firstLevelName);
    }

    public void RestartGame()
    {
        // GameController.instance.ResetGameVariables();
        SceneManager.LoadScene(_firstLevelName);
    }

    public void GameOver()
    {
        SceneManager.LoadScene(_lastScene);
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
        SceneManager.LoadScene(_instructionsSceneName);
    }

    public void ShowOptionsScene()
    {
        SceneManager.LoadScene(_optionsSceneName);
    }

    public void ShowCreditsScene()
    {
        SceneManager.LoadScene(_creditsSceneName);
    }

    public void SetCurrentSceneIndex()
    {
        _currentScene = SceneManager.GetActiveScene().buildIndex;
        _currentSceneName = SceneManager.GetSceneByBuildIndex(_currentScene).name;
    }

    public void SetCurrentSceneType()
    {
        if (_currentSceneName == _startSceneName)
        {
            _currentSceneType = SceneTypes.Start;
        }
        else if (_currentSceneName == _instructionsSceneName)
        {
            _currentSceneType = SceneTypes.Instructions;
        }
        else if (_currentSceneName == _optionsSceneName)
        {
            _currentSceneType = SceneTypes.Options;
        }
        else if (_currentSceneName == _creditsSceneName)
        {
            _currentSceneType = SceneTypes.Credits;
        }
        else if (_currentSceneName == _gameOverSceneName)
        {
            _currentSceneType = SceneTypes.GameOver;
        }
        else
        {
            _currentSceneType = SceneTypes.Level;
            Debug.Log("setting scene to level");
        }
    }

    protected override void AdditionalDestroyTasks()
    {
        // Remove the delegate when the object is destroyed
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    public SceneTypes currentSceneType
    {
        // set { SetCurrentSceneType(); }
        get { return _currentSceneType; }
    }
}
