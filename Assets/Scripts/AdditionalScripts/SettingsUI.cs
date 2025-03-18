using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button toggleSettingsButton;
    [SerializeField] private GameObject settingsPanel;
    
    private bool _isSettingsOpen = false;
    private void Awake()
    {
        toggleSettingsButton.onClick.AddListener(ToggleSettings);
        closeButton.onClick.AddListener(CloseSettings);
        settingsPanel.SetActive(false);
    }

    private void Start()
    {
        LoadVolumeSettings();

        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);

    }

    private void ToggleSettings()
    {
        _isSettingsOpen = !_isSettingsOpen;
        settingsPanel.SetActive(_isSettingsOpen);

        if (_isSettingsOpen)
        {
            LoadVolumeSettings();
        }
    }

    private void CloseSettings()
    {
        _isSettingsOpen = false;
        settingsPanel.SetActive(false);
    }
    
    private void LoadVolumeSettings()
    {
        float savedMusicVolume = GameData.Instance.MusicVolume;
        float savedSFXVolume = GameData.Instance.SoundVolume;

        musicVolumeSlider.value = savedMusicVolume;
        sfxVolumeSlider.value = savedSFXVolume;

        MusicManager.Instance.SetMusicVolume(savedMusicVolume);
        SoundEffectsManager.Instance.SetSFXVolume(savedSFXVolume);
    }

    private void OnMusicVolumeChanged(float volume)
    {
        MusicManager.Instance.SetMusicVolume(volume);
    }

    private void OnSFXVolumeChanged(float volume)
    {
        SoundEffectsManager.Instance.SetSFXVolume(volume);
    }
    
}