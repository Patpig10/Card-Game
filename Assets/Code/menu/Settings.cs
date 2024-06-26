using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;

    public AudioMixer audioMixer;
    public Slider MasterVol, MusicVol, SFXVol;

    private List<Resolution> predefinedResolutions;

    void Start()
    {
        // Define specific resolutions
        predefinedResolutions = new List<Resolution>
        {
            new Resolution { width = 640, height = 480 },
            new Resolution { width = 800, height = 600 },
            new Resolution { width = 1024, height = 768 },
            new Resolution { width = 1152, height = 864 },
            new Resolution { width = 1280, height = 600 },
            new Resolution { width = 1280, height = 720 },
            new Resolution { width = 1280, height = 1024 },
            new Resolution { width = 1920, height = 1080 }
        };

        // Populate resolution dropdown
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < predefinedResolutions.Count; i++)
        {
            string option = predefinedResolutions[i].width + " x " + predefinedResolutions[i].height;
            options.Add(option);

            if (predefinedResolutions[i].width == Screen.currentResolution.width &&
                predefinedResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Set the fullscreen toggle to the current screen mode
        fullscreenToggle.isOn = Screen.fullScreen;

        // Add listeners to UI elements
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
    }

    public void ChangeGraphQuality()
    {
        QualitySettings.SetQualityLevel(resolutionDropdown.value);
    }

    public void ChangeMasterVolume()
    {
        audioMixer.SetFloat("MasterVol", MasterVol.value);
    }

    public void ChangeMusicVolume()
    {
        audioMixer.SetFloat("MusicVol", MusicVol.value);
    }

    public void ChangeSFXVolume()
    {
        audioMixer.SetFloat("SFXVol", SFXVol.value);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        Debug.Log("Fullscreen mode set to: " + isFullscreen);
    }

    public void SetResolution(int resolutionIndex)
    {
        if (resolutionIndex < 0 || resolutionIndex >= predefinedResolutions.Count)
        {
            Debug.LogError("Invalid resolution index: " + resolutionIndex);
            return;
        }

        Resolution resolution = predefinedResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        Debug.Log("Resolution set to: " + resolution.width + " x " + resolution.height);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
