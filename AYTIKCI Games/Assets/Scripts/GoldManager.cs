using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        // Attach the scene loaded event listener
        SceneManager.sceneLoaded += OnSceneLoaded;
        // Load the player's gold when the game starts
        LoadGold();
        FindGoldDisplayInScene();
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
        Debug.Log(PlayerPrefs.GetInt(GOLD_KEY, 100));
        currentGold = PlayerPrefs.GetInt(GOLD_KEY, 100); // Load gold, default to 100 if not found
        Debug.Log(currentGold);
    }

    // Updates the gold display on the UI
    public void UpdateGoldDisplay()
    {
        if (goldDisplay != null)
        {
            goldDisplay.text = currentGold.ToString();  // Update the UI text
        }
    }

    private void OnDestroy()
    {
        // Detach the scene loaded event listener when the object is destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reassign goldDisplay when a new scene is loaded
        FindGoldDisplayInScene();
        UpdateGoldDisplay();
    }

    private void FindGoldDisplayInScene()
    {
        GameObject goldDisplayObject = GameObject.Find("goldDisplay");
        if (goldDisplayObject != null)
        {
            goldDisplay = goldDisplayObject.GetComponent<Text>();
        }
        else
        {
            Debug.LogWarning("GoldText UI element not found in the current scene.");
        }
    }

    public void ResetGold()
    {
        currentGold = 100; // Set to default value
        SaveGold(); // Save to PlayerPrefs
        UpdateGoldDisplay(); // Update the UI
    }
}