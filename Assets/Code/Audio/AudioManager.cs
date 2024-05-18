using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public class SceneAudio
    {
        public string sceneName;
        public AudioClip bgmClip;
    }

    public List<SceneAudio> sceneAudios;  // List to hold background music clips for each scene
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGMForScene(scene.name);
    }

    public void PlayBGMForScene(string sceneName)
    {
        foreach (SceneAudio sceneAudio in sceneAudios)
        {
            if (sceneAudio.sceneName == sceneName)
            {
                audioSource.clip = sceneAudio.bgmClip;
                audioSource.Play();
                return;
            }
        }
    }
}
