using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : Singleton<CanvasController> {

	protected CanvasController() {}

	[Header("Canvas Types")]
	[SerializeField]
	public GameObject startScreenCanvas;
	[SerializeField]
	private GameObject levelScreenCanvas;
	[SerializeField]
	private GameObject gameOverScreenCanvas;

	public GameObject btnStart;
	public GameObject btnQuit;
	public GameObject btnPlayAgain;
	public GameObject _txtCoins;
	public GameObject _txtLives;
	public GameObject _txtScore;
	public GameObject _txtTimer;

	public void EnableSceneCanvas () 
	{
		//ChooseCanvas();
		LinkCanvasElements();
	}

	private void ChooseCanvas()
	{
		switch (SceneController.instance.currentSceneType)
		{
			case SceneController.SceneTypes.Start:
			{
				startScreenCanvas.SetActive(true);
				levelScreenCanvas.SetActive(false);
				gameOverScreenCanvas.SetActive(false);
				break;
			}
			case SceneController.SceneTypes.GameOver:
			{
				startScreenCanvas.SetActive(false);
				levelScreenCanvas.SetActive(false);
				gameOverScreenCanvas.SetActive(true);
				break;
			}
			case SceneController.SceneTypes.Level:
			default:
			{
				startScreenCanvas.SetActive(false);
				levelScreenCanvas.SetActive(true);
				gameOverScreenCanvas.SetActive(false);
				break;
			}
		}
	}

	private void LinkCanvasElements()
	{
		btnStart = GameObject.Find("btnStart");
		if (btnStart)
		{
			btnStart.GetComponent<Button>().onClick.AddListener(SceneController.instance.StartGame);
		}

		btnQuit = GameObject.Find("btnQuit");
		if (btnQuit)
		{
			btnQuit.GetComponent<Button>().onClick.AddListener(SceneController.instance.QuitGame);
		}

		btnPlayAgain = GameObject.Find("btnPlayAgain");
		if (btnPlayAgain)
		{
			btnPlayAgain.GetComponent<Button>().onClick.AddListener(SceneController.instance.RestartGame);
		}

		_txtCoins = GameObject.Find("txtCoins");
		_txtLives = GameObject.Find("txtLives");
		_txtScore = GameObject.Find("txtScore");
		_txtTimer = GameObject.Find("txtTimer");
	}

	public Text txtCoins
	{
		get
		{ 
			if(_txtCoins)
			{
				return _txtCoins.GetComponent<Text>();
			}
			else
			{
				return null;
			}
		}
	}

	public Text txtLives
	{
		get
		{
			if (_txtLives)
			{
				return _txtLives.GetComponent<Text>();
			}
			else
			{
				return null;
			}
		}
	}

	public Text txtScore
	{
		get
		{
			if (_txtScore)
			{
				return _txtScore.GetComponent<Text>();
			}
			else
			{
				return null;
			}
		}
	}

	public Text txtTimer
	{
		get
		{
			if (_txtTimer)
			{
				return _txtTimer.GetComponent<Text>();
			}
			else
			{
				return null;
			}
		}
	}
}
