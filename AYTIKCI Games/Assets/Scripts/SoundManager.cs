using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioSource[] sfxSource; // Source for playing sound effects
    public AudioSource bgMusicSource; // Source for background music (optional)

    private float bgMusicVolume = 1.0f;
    private float sfxVolume = 1.0f;
    private float bgMusicTime = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += onSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        LoadVolumeSettings();
    }
    private void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignAudioSources();

        if (bgMusicSource != null)
        {
            bgMusicSource.time = bgMusicTime;
            if (!bgMusicSource.isPlaying)
            {
                bgMusicSource.Play();
            }

        }
        ApplyVolumeSettings();
    }
    private void AssignAudioSources()
    {

        if (bgMusicSource != null)
        {
            bgMusicSource.loop = true;
        }

        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        sfxSource = System.Array.FindAll(allAudioSources, source => source != bgMusicSource);

    }

    // Play a single sound effect
    public void PlaySound(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource[0].PlayOneShot(clip);
        }
    }

    // Play or loop music (optional)
    public void PlayMusic(AudioClip musicClip)
    {
        if (bgMusicSource != null && musicClip != null)
        {
            bgMusicSource.clip = musicClip;
            bgMusicSource.loop = true;
            bgMusicSource.Play();
        }
    }

    // Stop music playback (optional)
    public void StopMusic()
    {
        if (bgMusicSource != null)
        {
            bgMusicSource.Stop();
        }
    }
    private void LoadVolumeSettings()
    {
        bgMusicVolume = PlayerPrefs.GetFloat("BGMusicVolume", 1f);

        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }

    private void ApplyVolumeSettings()
    {
        if (bgMusicSource != null)
        {
            bgMusicSource.volume = bgMusicVolume;
        }

        foreach (var sfx in sfxSource)
        {
            if (sfx != null)
            {
                sfx.volume = sfxVolume;
            }
        }
    }
    public void SetBGMusicVolume(float volume)
    {
        bgMusicVolume = volume;
        PlayerPrefs.SetFloat("BGMusicVolume", bgMusicVolume);
        ApplyVolumeSettings();
    }
    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        ApplyVolumeSettings();
    }
}
