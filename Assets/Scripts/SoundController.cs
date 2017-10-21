using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : Singleton<SoundController> {

	protected SoundController() {}

	[Header("AudioSources")]
	[SerializeField]
		private AudioSource sfxPlayer;
	[SerializeField]
		private AudioSource sfxSecondaryPlayer;
    [SerializeField]
		private AudioSource musicPlayer;
    
	[Header("Music")]
	[SerializeField]
		private AudioClip musicStart;
	[SerializeField]
		private AudioClip musicLevel;
	[SerializeField]
		private AudioClip _musicHurryUp;
	[SerializeField]
		private AudioClip musicGameOver;

	[Header("SFX")]
	[SerializeField]
		private AudioClip _sfxGainLife;
    [SerializeField]
		private AudioClip _sfxLoseLife;
	[SerializeField]
		private AudioClip _sfxLosePower;
    [SerializeField]
		private AudioClip _sfxGetBig;
    [SerializeField]
		private AudioClip _sfxGetRacoon;
	[SerializeField]
		private AudioClip _sfxJump;
    [SerializeField]
		private AudioClip _sfxGetCoin;
	[SerializeField]
		private AudioClip _sfxHitBlock;	
	[SerializeField]
		private AudioClip _sfxSquish;

    public void PlaySFX(AudioClip clip, float volume = 0.5f)
    {
        AudioSource player;
		if (!sfxPlayer.isPlaying)
		{
			player = sfxPlayer;
		}
		else
		{
			player = sfxSecondaryPlayer;
		}
		
		// adjust the AudioSource volume
        player.volume = volume;
        
        // assign the clip to the AudioSource
        player.clip = clip;

        // Play the AudioSource
        player.Play();
    }

    public void PlayMusic(AudioClip clip, float volume = 1.0f)
    {
        // adjust the AudioSource volume
        musicPlayer.volume = volume;
        
        // assign the clip to the AudioSource
        musicPlayer.clip = clip;

        // Play the AudioSource
        musicPlayer.Play();
    }

	public void EnableSceneMusic()
	{
		switch (SceneController.instance.currentSceneType)
		{
			case SceneController.SceneTypes.Start:
			{
				PlayMusic(musicStart);
				break;
			}
			case SceneController.SceneTypes.GameOver:
			{
				PlayMusic(musicGameOver);
				break;
			}
			case SceneController.SceneTypes.Level:
			default:
			{
				PlayMusic(musicLevel);
				break;
			}
		}
	}

	public AudioClip MusicPlaying()
	{
		return musicPlayer.clip;
	}

	public AudioClip musicHurryUp
	{
		get { return _musicHurryUp;}
	}

	public AudioClip sfxGainLife
	{
		get { return _sfxGainLife; }
	}

	public AudioClip sfxLoseLife
	{
		get { return _sfxLoseLife; }
	}

	public AudioClip sfxLosePower
	{
		get { return _sfxLosePower; }
	}
    
	public AudioClip sfxGetBig
	{
		get { return _sfxGetBig; }
	}
    
	public AudioClip sfxGetRacoon
	{
		get { return _sfxGetRacoon; }
	}
	
	public AudioClip sfxJump
	{
		get { return _sfxJump; }
	}
    
	public AudioClip sfxGetCoin
	{
		get { return _sfxGetCoin; }
	}
	
	public AudioClip sfxHitBlock
	{
		get { return _sfxHitBlock; }
	}

	public AudioClip sfxSquish
	{
		get { return _sfxSquish; }
	}
}
