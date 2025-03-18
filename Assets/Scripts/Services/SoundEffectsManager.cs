using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{
    public static SoundEffectsManager Instance { get; private set; }

    [Header("Sound Collection")]
    [SerializeField] private SoundEffectCollection soundEffectCollection;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private AudioSource sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupAudioSource();
            LoadSFXVolume();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SetupAudioSource()
    {
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.volume = sfxVolume;
    }

    public void PlaySound(string soundName)
    {
        AudioClip clip = soundEffectCollection.GetSound(soundName);
        if (soundName == "victorySound" || soundName == "defeatSound")
        {
            MusicManager.Instance.PauseMusic();
        }
        if (clip == null)
        {
            Debug.LogWarning($"Sound '{soundName}' not found in the collection.");
            return;
        }

        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void StopAllSounds()
    {
        if (sfxSource != null)
        {
            sfxSource.Stop();
        }
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
        }
        SaveSFXVolume(sfxVolume);
    }

    private void SaveSFXVolume(float volume)
    {
        GameData.Instance.SaveSoundsVolume(volume);
    }

    private void LoadSFXVolume()
    {
        sfxVolume = SaveSystem.Load().SoundVolume;
        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
        }
    }
}
