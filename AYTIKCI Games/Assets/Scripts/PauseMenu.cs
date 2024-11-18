using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StopTime()
    {
        // Pause the game
        Time.timeScale = 0;

    }
    public void StartTime()
    {
        // Pause the game
        Time.timeScale = 1;

    }

    public void ReturnToMainMenu()
    {
        // Reset the time scale in case it was paused
        Time.timeScale = 1;

        // Load the main menu scene (replace "MainMenu" with your scene name)
        SceneManager.LoadScene("MainMenu");
    }
}
