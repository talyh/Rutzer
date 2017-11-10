using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundController : Singleton<SoundController>
{

    public const float MAX_VOLUME = 0;
    public const float MIN_VOLUME = -80;

    private const string MIXER_SFX_CHANNEL = "SFX";

    [Header("Audio Objects")]
    [SerializeField]
    private AudioMixer _masterMixer;
    [SerializeField]
    private AudioSource _musicPlayer;
    [SerializeField]
    private AudioSource _primarySFXPlayer;
    private AudioSource _secondarySFXPlayer;

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
    private AudioClip _sfxIncreaseSpeed;
    [SerializeField]
    private AudioClip _sfxDie;
    [SerializeField]
    private AudioClip _sfxStretchPlatform;
    [SerializeField]
    private AudioClip _sfxMovePlatform;

    public void PlaySFX(AudioClip clip)
    {
        if (_primarySFXPlayer.mute)
        {
            return;
        }

        AudioSource player;
        if (!_primarySFXPlayer.isPlaying)
        {
            Supporting.Log("Using primary audio source to play: " + clip.name);
            player = _primarySFXPlayer;
        }
        else
        {
            Supporting.Log("Using secondary audio source to play " + clip.name);
            if (!_secondarySFXPlayer)
            {
                _secondarySFXPlayer = _primarySFXPlayer.gameObject.AddComponent<AudioSource>();
                _secondarySFXPlayer.outputAudioMixerGroup = _masterMixer.FindMatchingGroups(MIXER_SFX_CHANNEL)[0];
            }

            player = _secondarySFXPlayer;
        }

        // assign the clip to the AudioSource
        player.clip = clip;

        // Play the AudioSource
        player.Play();
    }

    public void PlayMusic(AudioClip clip)
    {
        if (_musicPlayer.mute)
        {
            return;
        }

        // assign the clip to the AudioSource
        _musicPlayer.clip = clip;

        // Play the AudioSource
        _musicPlayer.Play();
    }

    public void EnableSceneMusic()
    {
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
            // Supporting.Log("Adjusting music volume to " + volume);
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
        _primarySFXPlayer.mute = !enabled;
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
        get { return _primarySFXPlayer.mute; }
    }

    public AudioClip sfxJump
    {
        get { return _sfxJump; }
    }

    public AudioClip sfxDie
    {
        get { return _sfxDie; }
    }

    public AudioClip sfxIncreaseSpeed
    {
        get { return _sfxIncreaseSpeed; }
    }

    public AudioClip sfxStretchPlatform
    {
        get { return _sfxStretchPlatform; }
    }

    public AudioClip sfxMovePlatform
    {
        get { return _sfxMovePlatform; }
    }
}
