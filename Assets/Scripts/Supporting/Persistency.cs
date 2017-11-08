using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Persistency
{
    public enum DataGroups { Sound, Score }

    public const string MUSIC_VOLUME_KEY = "MUSIC_VOLUME";
    public const string MUSIC_MUTED_KEY = "MUSIC_MUTED";
    public const string SFX_VOLUME_KEY = "SFX_VOLUME";
    public const string SFX_MUTED_KEY = "SFX_MUTED";
    public const string HIGHSCORE_KEY = "HIGHSCORE";

    public static void LoadSavedData(DataGroups dataGroup)
    {
        // Supporting.Log("Loading Saved Data");
        switch (dataGroup)
        {
            case DataGroups.Sound:
                {
                    float musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0);
                    bool musicMuted = PlayerPrefs.GetInt(MUSIC_MUTED_KEY, 0) >= 1 ? true : false;
                    // adjust Audio
                    SoundController.instance.SetMusicVolume(musicVolume);
                    SoundController.instance.SetMusicVolume(!musicMuted);
                    // adjust Canvas elements
                    CanvasController.instance.ShowMusicVolume(musicVolume);
                    CanvasController.instance.ToggleMusicVolume(!musicMuted);

                    float SFXVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 0);
                    bool SFXMuted = PlayerPrefs.GetInt(SFX_MUTED_KEY, 0) >= 1 ? true : false;
                    // adjust Audio
                    SoundController.instance.SetSFXVolume(SFXVolume);
                    SoundController.instance.SetSFXVolume(!SFXMuted);
                    // adjust Canvas elements
                    CanvasController.instance.ShowSFXVolume(SFXVolume);
                    CanvasController.instance.ToggleSFXVolume(!SFXMuted);
                    break;
                }
            case DataGroups.Score:
                {
                    GameController.instance.highScore = PlayerPrefs.GetInt(HIGHSCORE_KEY, 0);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    public static void SaveData(DataGroups dataGroup)
    {
        switch (dataGroup)
        {
            case DataGroups.Sound:
                {
                    float musicVolume = 0;
                    SoundController.instance.masterMixer.GetFloat(MUSIC_VOLUME_KEY, out musicVolume);
                    PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, musicVolume);
                    // Supporting.Log(string.Format("Saved {0}: {1}", MUSIC_VOLUME_KEY, PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY)));
                    PlayerPrefs.SetInt(MUSIC_MUTED_KEY, SoundController.instance.musicMuted ? 1 : 0);
                    // Supporting.Log(string.Format("Saved {0}: {1}", MUSIC_MUTED_KEY, PlayerPrefs.GetInt(MUSIC_MUTED_KEY)));

                    float SFXVolume = 0;
                    SoundController.instance.masterMixer.GetFloat(SFX_VOLUME_KEY, out SFXVolume);
                    PlayerPrefs.SetFloat(SFX_VOLUME_KEY, SFXVolume);
                    // Supporting.Log(string.Format("Saved {0}: {1}", MUSIC_VOLUME_KEY, PlayerPrefs.GetFloat(SFX_VOLUME_KEY)));
                    PlayerPrefs.SetInt(SFX_MUTED_KEY, SoundController.instance.SFXMuted ? 1 : 0);
                    // Supporting.Log(string.Format("Saved {0}: {1}", SFX_MUTED_KEY, PlayerPrefs.GetFloat(SFX_MUTED_KEY)));
                    break;
                }
            case DataGroups.Score:
                {
                    PlayerPrefs.SetInt(HIGHSCORE_KEY, GameController.instance.highScore);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}
