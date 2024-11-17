using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUHealth : MonoBehaviour
{
    public float health = 500f;
    public GameObject eliminationMenu;
    public GameObject turretMenu;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
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
