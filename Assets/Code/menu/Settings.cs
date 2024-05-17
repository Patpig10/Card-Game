using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;

    public AudioMixer audioMixer;
    public Slider MasterVol, MusicVol, SFXVol;

    public void ChangeGraphQuality()
    {
        QualitySettings.SetQualityLevel(resolutionDropdown.value);
    }
    // Start is called before the first frame update
    void Start()
    {
        
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
    // Update is called once per frame
    void Update()
    {
        
    }
}
