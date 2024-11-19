using UnityEngine;
using UnityEngine.SceneManagement;

public class EliminationMenu : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        // Reset the time scale in case it was paused
        Time.timeScale = 1;
        GoldManager.Instance.ResetGold();
        // Load the main menu scene (replace "MainMenu" with your scene name)
        SceneManager.LoadScene("MainMenu");
    }
}
