using UnityEngine;
using UnityEngine.UI;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance; // Singleton instance

    public int currentGold = 100; // Default starting gold
    public Text goldDisplay; // UI Text to display current gold

    private const string GOLD_KEY = "PlayerGold"; // Key used to store gold in PlayerPrefs

    void Awake()
    {
        // Ensure there is only one instance of GoldManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps this object across scenes
        }
        else
        {
            Destroy(gameObject); // Destroys duplicates
        }
    }

    void Start()
    {
        // Load the player's gold when the game starts
        LoadGold();
        UpdateGoldDisplay();
    }

    // Adds gold to the player's account
    public void AddGold(int amount)
    {
        currentGold += amount;
        SaveGold(); // Save to PlayerPrefs whenever gold is added
        UpdateGoldDisplay();
    }

    // Subtracts gold from the player's account and checks if they have enough
    public bool SpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            SaveGold(); // Save to PlayerPrefs whenever gold is spent
            UpdateGoldDisplay();
            return true;
        }
        else
        {
            Debug.Log("Not enough gold!"); // Not enough gold to spend
            return false;
        }
    }

    // Saves the current gold to PlayerPrefs
    private void SaveGold()
    {
        PlayerPrefs.SetInt(GOLD_KEY, currentGold); // Store the gold value
        PlayerPrefs.Save(); // Ensure that the data is saved immediately
    }

    // Loads the player's gold from PlayerPrefs
    private void LoadGold()
    {
        currentGold = PlayerPrefs.GetInt(GOLD_KEY, 100); // Load gold, default to 100 if not found
    }

    // Updates the gold display on the UI
    public void UpdateGoldDisplay()
    {
        if (goldDisplay != null)
        {
            goldDisplay.text = currentGold.ToString();  // Update the UI text
        }
    }
}