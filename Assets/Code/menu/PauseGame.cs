using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PauseGame : MonoBehaviour
{
    public GameObject pauseMenuUI; // Assign the pause menu UI Panel here

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume game time
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Pause game time
        isPaused = true;
    }

    // Optional: Add methods for other pause menu buttons, like Restart and Quit
    public void Restart()
    {
        Time.timeScale = 1f; // Ensure time scale is reset before reloading
        // Add your scene reload logic here, e.g., SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Time.timeScale = 1f; // Ensure time scale is reset before quitting
        // Add your quit logic here, e.g., Application.Quit();
    }
}
