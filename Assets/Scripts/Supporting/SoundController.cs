using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundController : Singleton<SoundController>
{

    public const float MAX_VOLUME = 0;
    public const float MIN_VOLUME = -80;

    [Header("Audio Objects")]
    [SerializeField]
    private AudioMixer _masterMixer;
    [SerializeField]
    private AudioSource _musicPlayer;
    [SerializeField]
    private AudioSource _sfxPlayer;

    [Header("Music")]
    [SerializeField]
    private AudioClip _musicStart;
    [SerializeField]
    private AudioClip _musicLevel;
    [SerializeField]
    private AudioClip musicGameOver;

    [Header("SFX")]
    [SerializeField]
    private AudioClip _sfxJump;
    [SerializeField]
    private AudioClip _sfxStretchPlatform;
    [SerializeField]
    private AudioClip _sfxMovePlatform;
    [SerializeField]
    private AudioClip _sfxDie;

    public void PlaySFX(AudioClip clip)
    {
        AudioSource player;
        if (!_sfxPlayer.isPlaying)
        {
            player = _sfxPlayer;
        }
        else
        {
            player = new AudioSource();
        }

        // assign the clip to the AudioSource
        player.clip = clip;

        // Play the AudioSource
        player.Play();
    }

    public void PlayMusic(AudioClip clip)
    {
        // assign the clip to the AudioSource
        _musicPlayer.clip = clip;

        // Play the AudioSource
        _musicPlayer.Play();
    }

    public void EnableSceneMusic()
    {
        Debug.Log("current scene type: " + SceneController.instance.currentSceneType);
        switch (SceneController.instance.currentSceneType)
        {
            case SceneController.SceneTypes.Start:
                {
                    PlayMusic(_musicStart);
                    break;
                }
            case SceneController.SceneTypes.GameOver:
                {
                    PlayMusic(musicGameOver);
                    break;
                }
            case SceneController.SceneTypes.Instructions:
            case SceneController.SceneTypes.Options:
            case SceneController.SceneTypes.Credits:
                {
                    break;
                }
            case SceneController.SceneTypes.Level:
            default:
                {
                    PlayMusic(_musicLevel);
                    break;
                }
        }
    }


    public void SetMusicVolume(float volume)
    {
        if (_masterMixer)
        {
            Supporting.Log("Adjusting music volume to " + volume);
            _masterMixer.SetFloat(Persistency.MUSIC_VOLUME_KEY, volume);
        }
        else
        {
            Supporting.Log("Master Mixer not found", 2);
        }
    }

    public void SetMusicVolume(bool enabled)
    {
        _musicPlayer.mute = !enabled;
    }

    public void SetSFXVolume(float volume)
    {
        _masterMixer.SetFloat(Persistency.SFX_VOLUME_KEY, volume);
    }

    public void SetSFXVolume(bool enabled)
    {
        _sfxPlayer.mute = !enabled;
    }

    public AudioMixer masterMixer
    {
        get { return _masterMixer; }
    }

    public bool musicMuted
    {
        get { return _musicPlayer.mute; }
    }

    public bool SFXMuted
    {
        get { return _sfxPlayer.mute; }
    }

    public AudioClip sfxJump
    {
        get { return _sfxJump; }
    }

    public AudioClip sfxStretchPlatform
    {
        get { return _sfxStretchPlatform; }
    }

    public AudioClip sfxMovePlatform
    {
        get { return _sfxMovePlatform; }
    }

    public AudioClip sfxDie
    {
        get { return _sfxDie; }
    }
}
