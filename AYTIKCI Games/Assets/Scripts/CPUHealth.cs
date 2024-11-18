using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CPUHealth : MonoBehaviour
{
    public float health = 500f;
    public GameObject eliminationMenu;
    public GameObject turretMenu;
    public Text healthDisplay; // UI Text to display current gold

    public AudioClip defeatSound; // Sound effect for the turret attack

    void Update()
    {
        UpdateHealthDisplay();
    }

    public void UpdateHealthDisplay()
    {
        if (healthDisplay != null)
        {
            healthDisplay.text = health.ToString();  // Update the UI text
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            if (defeatSound != null)
            {
                GoldManager.Instance.ResetGold();
                SoundManager.Instance.PlaySound(defeatSound);
            }
            ShowEliminationMenu(); // Show the menu
            Destroy(gameObject); // Destroy CPU when health is 0
        }
    }
    void ShowEliminationMenu()
    {
        // Pause the game
        Time.timeScale = 0;

        // Enable the elimination menu
        if (eliminationMenu != null)
        {
            turretMenu.SetActive(false);
            eliminationMenu.SetActive(true);
        }
    }
}
