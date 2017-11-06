﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : Singleton<CanvasController>
{
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

    [Header("Button types")]
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


    // ----- ELEMENTS THAT MAY EXIST ACROSS MULTIPLE CANVASES, DEPENDING ON SCENE
    // These will be dynamically binded as needed
    private Button _btnStart;
    private Button _btnInstructions;
    private Button _btnOptions;
    private Button _btnCredits;
    private Button _btnQuit;
    private Button _btnPause;

    public void EnableSceneCanvas()
    {
        ChooseCanvas();
        LinkCanvasElements();
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
                _btnStart.onClick.AddListener(SceneController.instance.StartGame);
            }
            else if (button.name == _btnInstructionsName)
            {
                _btnInstructions = button;
                _btnInstructions.onClick.AddListener(SceneController.instance.ShowInstructionsScene);
            }
            else if (button.name == _btnOptionsName)
            {
                _btnOptions = button;
                _btnOptions.onClick.AddListener(SceneController.instance.ShowOptionsScene);
            }
            else if (button.name == _btnCreditsName)
            {
                _btnCredits = button;
                _btnCredits.onClick.AddListener(SceneController.instance.ShowCreditsScene);
            }
            else if (button.name == _btnQuitName)
            {
                _btnQuit = button;
                _btnQuit.onClick.AddListener(SceneController.instance.QuitGame);
            }
            else if (button.name == _btnPauseName)
            {
                _btnPause = button;
                _btnPause.onClick.AddListener(GameController.instance.PauseGame);
            }
            else
            {
                Debug.LogError(string.Format("Could not resolve button {0}", button));
            }
        }
    }
}