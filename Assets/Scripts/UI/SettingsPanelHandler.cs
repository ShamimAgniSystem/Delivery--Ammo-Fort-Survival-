using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsPanelHandler : MonoBehaviour
{
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle SFXToggle;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioMixer audioMixer;
    
    [SerializeField] private Button backButton;
    [SerializeField] private Button applyButton;

    private void Start()
    {
        backButton.onClick.AddListener(OnClickBackButton);
        applyButton.onClick.AddListener(OnClickApplyButton);
        
        LoadSettings();
    }

    public void SetMusic(bool isOn)
    {
        audioMixer.SetFloat("MusicVolume", isOn ? 0f : -80f);
        PlayerPrefs.SetInt("Music", isOn ? 1 : 0);
    }
    public void SetSFX(bool isOn)
    {
        audioMixer.SetFloat("SFXVolume", isOn ? 0f : -80f);
        PlayerPrefs.SetInt("SFX", isOn ? 1 : 0);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat("Volume", Mathf.Log10(volume) * 20f);
    }
    
    public void LoadSettings()
    {
        musicToggle.isOn = PlayerPrefs.GetInt("Music", 1) == 1;
        SFXToggle.isOn = PlayerPrefs.GetInt("SFX", 1) == 1;
        volumeSlider.value = PlayerPrefs.GetInt("Volume", 1);
    }
    private void OnClickApplyButton()
    {
        SetMusic(musicToggle.isOn);
        SetSFX(SFXToggle.isOn);
        SetVolume(volumeSlider.value);
    }

    private void OnClickBackButton()
    {
        gameObject.SetActive(false);
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Transform[] allTransforms = FindObjectsByType<Transform>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            GameObject mainMenuGroup = null;

            foreach (Transform t in allTransforms)
            {
                if (t.name == "MainMenuGroup")
                {
                    mainMenuGroup = t.gameObject;
                    break; 
                }
            }
            if (mainMenuGroup != null)
            {
                mainMenuGroup.SetActive(true);
            }
            else
            {
                Debug.LogWarning("MainMenuGroup GameObject not found in the scene.");
            }
        }
    }
}
