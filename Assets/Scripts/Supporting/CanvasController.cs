﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CanvasController : Singleton<CanvasController>
{
    private const string TEXT_ELEMENTS_PREFIX = "txt";

    [Header("Canvas Types")]
    [SerializeField]
    private GameObject _startScreenCanvas;
    [SerializeField]
    private GameObject _instructionsScreenCanvas;
    [SerializeField]
    private GameObject _optionsScreenCanvas;
    [SerializeField]
    private GameObject _creditsScreenCanvas;
    [SerializeField]
    private GameObject _levelScreenCanvas;
    [SerializeField]
    private GameObject _gameOverScreenCanvas;
    [SerializeField]
    private GameObject _pauseMenuCanvas;

    [Header("UI Elements")]
    [Header("Buttons")]
    [SerializeField]
    private string _btnStartName = "btnStart";
    [SerializeField]
    private string _btnInstructionsName = "btnInstructions";
    [SerializeField]
    private string _btnOptionsName = "btnOptions";
    [SerializeField]
    private string _btnCreditsName = "btnCredits";
    [SerializeField]
    private string _btnQuitName = "btnQuit";
    [SerializeField]
    private string _btnPauseName = "btnPause";
    [SerializeField]
    private string _btnReturnName = "btnReturn";
    [SerializeField]
    private string _btnPlayAgainName = "btnPlayAgain";

    [Header("Sliders")]
    [SerializeField]
    private string _sldMusicVolumeName = "sldMusicVolume";
    [SerializeField]
    private string _sldSFXVolumeName = "sldSFXVolume";

    [Header("Toggles")]
    [SerializeField]
    private string _optMusicVolumeName = "optMusicVolume";
    [SerializeField]
    private string _optSFXVolumeName = "optSFXVolume";

    [Header("Texts")]
    [SerializeField]
    private string _txtSpeedName = "txtSpeed";
    [SerializeField]
    private string _txtScoreName = "txtScore";
    [SerializeField]
    private string _txtHighScoreName = "txtHighScore";
    [SerializeField]
    private string _txtWinName = "txtWin";
    [SerializeField]
    private string _txtLoseName = "txtLose";


    // ----- ELEMENTS THAT MAY EXIST ACROSS MULTIPLE CANVASES, DEPENDING ON SCENE
    // These will be dynamically binded as needed
    private Button _btnStart;
    private Button _btnInstructions;
    private Button _btnOptions;
    private Button _btnCredits;
    private Button _btnQuit;
    private Button _btnPause;
    private Button _btnReturn;
    private Button _btnPlayAgain;
    private Toggle _optMusicVolume;
    private Slider _sldMusicVolume;
    private Toggle _optSFXVolume;
    private Slider _sldSFXVolume;
    private Text _txtSpeed;
    private Text _txtScore;
    private Text _txtHighScore;
    private Text _txtWin;
    private Text _txtLose;

    public void EnableSceneCanvas()
    {
        ChooseCanvas();
        LinkCanvasElements();
        Persistency.LoadSavedData(Persistency.DataGroups.Sound);
    }

    private void ChooseCanvas()
    {
        // enable the appropriate canvas based on Scene type, or hide them all if the Scene type is not recognized

        switch (SceneController.instance.currentSceneType)
        {
            case SceneController.SceneTypes.Start:
                {
                    _startScreenCanvas.SetActive(true);
                    _instructionsScreenCanvas.SetActive(false);
                    _optionsScreenCanvas.SetActive(false);
                    _creditsScreenCanvas.SetActive(false);
                    _levelScreenCanvas.SetActive(false);
                    _gameOverScreenCanvas.SetActive(false);
                    _pauseMenuCanvas.SetActive(false);
                    break;
                }
            case SceneController.SceneTypes.Instructions:
                {
                    _startScreenCanvas.SetActive(false);
                    _instructionsScreenCanvas.SetActive(true);
                    _optionsScreenCanvas.SetActive(false);
                    _creditsScreenCanvas.SetActive(false);
                    _levelScreenCanvas.SetActive(false);
                    _gameOverScreenCanvas.SetActive(false);
                    _pauseMenuCanvas.SetActive(false);
                    break;
                }
            case SceneController.SceneTypes.Options:
                {
                    _startScreenCanvas.SetActive(false);
                    _instructionsScreenCanvas.SetActive(false);
                    _optionsScreenCanvas.SetActive(true);
                    _creditsScreenCanvas.SetActive(false);
                    _levelScreenCanvas.SetActive(false);
                    _gameOverScreenCanvas.SetActive(false);
                    _pauseMenuCanvas.SetActive(false);
                    break;
                }
            case SceneController.SceneTypes.Credits:
                {
                    _startScreenCanvas.SetActive(false);
                    _instructionsScreenCanvas.SetActive(false);
                    _optionsScreenCanvas.SetActive(false);
                    _creditsScreenCanvas.SetActive(true);
                    _levelScreenCanvas.SetActive(false);
                    _gameOverScreenCanvas.SetActive(false);
                    _pauseMenuCanvas.SetActive(false);
                    break;
                }
            case SceneController.SceneTypes.Level:
                {
                    _startScreenCanvas.SetActive(false);
                    _instructionsScreenCanvas.SetActive(false);
                    _optionsScreenCanvas.SetActive(false);
                    _creditsScreenCanvas.SetActive(false);
                    _levelScreenCanvas.SetActive(true);
                    _gameOverScreenCanvas.SetActive(false);
                    _pauseMenuCanvas.SetActive(false);
                    Persistency.LoadSavedData(Persistency.DataGroups.Score);
                    break;
                }
            case SceneController.SceneTypes.GameOver:
                {
                    _startScreenCanvas.SetActive(false);
                    _instructionsScreenCanvas.SetActive(false);
                    _optionsScreenCanvas.SetActive(false);
                    _creditsScreenCanvas.SetActive(false);
                    _levelScreenCanvas.SetActive(false);
                    _gameOverScreenCanvas.SetActive(true);
                    _pauseMenuCanvas.SetActive(false);
                    ShowGameOverResult(GameController.instance.newRecord);
                    break;
                }
            default:
                {
                    _startScreenCanvas.SetActive(false);
                    _instructionsScreenCanvas.SetActive(false);
                    _optionsScreenCanvas.SetActive(false);
                    _creditsScreenCanvas.SetActive(false);
                    _levelScreenCanvas.SetActive(false);
                    _gameOverScreenCanvas.SetActive(false);
                    _pauseMenuCanvas.SetActive(false);
                    break;
                }
        }
    }

    public void EnablePauseCanvas(bool display)
    {
        _pauseMenuCanvas.SetActive(display);
        LinkCanvasElements();
    }

    private void LinkCanvasElements()
    {
        // look for the Canvas elements and bind them, as needed

        Button[] buttons = GameObject.FindObjectsOfType<Button>();
        foreach (Button button in buttons)
        {
            if (button.name == _btnStartName)
            {
                _btnStart = button;
                _btnStart.onClick.RemoveAllListeners();
                _btnStart.onClick.AddListener(SceneController.instance.StartGame);
            }
            else if (button.name == _btnInstructionsName)
            {
                _btnInstructions = button;
                _btnInstructions.onClick.RemoveAllListeners();
                _btnInstructions.onClick.AddListener(SceneController.instance.ShowInstructionsScene);
            }
            else if (button.name == _btnOptionsName)
            {
                _btnOptions = button;
                _btnOptions.onClick.RemoveAllListeners();
                _btnOptions.onClick.AddListener(SceneController.instance.ShowOptionsScene);
            }
            else if (button.name == _btnCreditsName)
            {
                _btnCredits = button;
                _btnCredits.onClick.RemoveAllListeners();
                _btnCredits.onClick.AddListener(SceneController.instance.ShowCreditsScene);
            }
            else if (button.name == _btnQuitName)
            {
                _btnQuit = button;
                _btnQuit.onClick.RemoveAllListeners();
                _btnQuit.onClick.AddListener(SceneController.instance.QuitGame);
            }
            else if (button.name == _btnPauseName)
            {
                _btnPause = button;
                _btnPause.onClick.RemoveAllListeners();
                _btnPause.onClick.AddListener(GameController.instance.PauseGame);
            }
            else if (button.name == _btnReturnName)
            {
                _btnReturn = button;
                _btnReturn.onClick.RemoveAllListeners();
                _btnReturn.onClick.AddListener(SceneController.instance.Return);
            }
            else if (button.name == _btnPlayAgainName)
            {
                _btnPlayAgain = button;
                _btnPlayAgain.onClick.RemoveAllListeners();
                _btnPlayAgain.onClick.AddListener(SceneController.instance.RestartGame);
            }
            else
            {
                Supporting.Log((string.Format("Could not resolve object {0} function", button)), 1);
            }
        }

        Slider[] sliders = GameObject.FindObjectsOfType<Slider>();
        foreach (Slider slider in sliders)
        {
            if (slider.name == _sldMusicVolumeName)
            {
                _sldMusicVolume = slider;
                _sldMusicVolume.onValueChanged.RemoveAllListeners();
                _sldMusicVolume.onValueChanged.AddListener(delegate { SoundController.instance.SetMusicVolume(_sldMusicVolume.value); });

            }
            else if (slider.name == _sldSFXVolumeName)
            {
                _sldSFXVolume = slider;
                _sldSFXVolume.onValueChanged.RemoveAllListeners();
                _sldSFXVolume.onValueChanged.AddListener(delegate { SoundController.instance.SetSFXVolume(_sldSFXVolume.value); });
            }
            else
            {
                Supporting.Log((string.Format("Could not resolve object {0} function", slider)), 1);
            }
        }

        Toggle[] toggles = GameObject.FindObjectsOfType<Toggle>();
        foreach (Toggle toggle in toggles)
        {
            if (toggle.name == _optMusicVolumeName)
            {
                _optMusicVolume = toggle;
                _optMusicVolume.onValueChanged.RemoveAllListeners();
                _optMusicVolume.onValueChanged.AddListener(delegate { SoundController.instance.SetMusicVolume(_optMusicVolume.isOn); });
            }
            else if (toggle.name == _optSFXVolumeName)
            {
                _optSFXVolume = toggle;
                _optSFXVolume.onValueChanged.RemoveAllListeners();
                _optSFXVolume.onValueChanged.AddListener(delegate { SoundController.instance.SetSFXVolume(_optSFXVolume.isOn); });
            }
            else
            {
                Supporting.Log((string.Format("Could not resolve object {0} function", toggle)), 1);
            }
        }

        Text[] texts = GameObject.FindObjectsOfType<Text>();
        foreach (Text text in texts)
        {
            // only go through elements that are TextAreas, as Unity's function will also return Text elements associated with
            // and other elements
            if (text.name.StartsWith(TEXT_ELEMENTS_PREFIX))
            {
                if (text.name == _txtSpeedName)
                {
                    _txtSpeed = text;
                    ShowSpeed((int)GameController.instance.speed);
                }
                else if (text.name == _txtScoreName)
                {
                    _txtScore = text;
                    ShowScore(GameController.instance.score);
                }
                else if (text.name == _txtHighScoreName)
                {
                    _txtHighScore = text;
                    ShowHighScore(GameController.instance.highScore);
                }
                else if (text.name == _txtWinName)
                {
                    _txtWin = text;
                }
                else if (text.name == _txtLoseName)
                {
                    _txtLose = text;
                }
                else
                {
                    Supporting.Log((string.Format("Could not resolve object {0} function", text)), 1);
                }
            }
        }
    }

    public void ShowMusicVolume(float value)
    {
        if (_sldMusicVolume)
        {
            _sldMusicVolume.value = value;
        }
    }

    public void ShowSFXVolume(float value)
    {
        if (_sldSFXVolume)
        {
            _sldSFXVolume.value = value;
        }
    }

    public void ToggleMusicVolume(bool enabled)
    {
        if (_optMusicVolume)
        {
            _optMusicVolume.isOn = enabled;
        }
    }

    public void ToggleSFXVolume(bool enabled)
    {
        if (_optSFXVolume)
        {
            _optSFXVolume.isOn = enabled;
        }
    }

    public void ShowSpeed(int speed)
    {
        if (_txtSpeed)
        {
            _txtSpeed.text = speed.ToString();
        }
    }

    public void ShowScore(int score)
    {
        if (_txtScore)
        {
            _txtScore.text = score.ToString();
        }
    }

    public void ShowHighScore(int highScore)
    {
        if (_txtHighScore)
        {
            _txtHighScore.text = highScore.ToString();
        }
    }

    private void ShowGameOverResult(bool newRecord = false)
    {
        // ensure binding, in case assynchronicty gets in the way
        if ((newRecord && !_txtWin) || (!newRecord && !_txtLose))
        {
            LinkCanvasElements();
        }

        if (newRecord)
        {
            _txtWin.text += GameController.instance.highScore;
            _txtWin.gameObject.SetActive(true);
            _txtLose.gameObject.SetActive(false);
        }
        else
        {
            _txtWin.gameObject.SetActive(false);
            _txtLose.gameObject.SetActive(true);
        }
    }
}
